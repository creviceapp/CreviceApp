﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Crevice.Core.FSM
{
    using System.Drawing;
    using System.Threading.Tasks;
    using Crevice.Core.Events;
    using Crevice.Core.Context;
    using Crevice.Core.DSL;
    using Crevice.Core.Stroke;
    using Crevice.Core.Helpers;

    public class GestureMachineConfig
    {
        // ms
        public int GestureTimeout { get; set; } = 1000;
        // px
        public int StrokeStartThreshold { get; set; } = 10;
        // px
        public int StrokeDirectionChangeThreshold { get; set; } = 20;
        // px
        public int StrokeExtensionThreshold { get; set; } = 10;
        // ms
        public int StrokeWatchInterval { get; set; } = 10;
    }

    public abstract class GestureMachine<TConfig, TContextManager, TEvalContext, TExecContext>
        : IIsDisposed, IDisposable
        where TConfig : GestureMachineConfig
        where TContextManager : ContextManager<TEvalContext, TExecContext>
        where TEvalContext : EvaluationContext
        where TExecContext : ExecutionContext
    {
        public readonly TConfig Config;
        public readonly TContextManager ContextManager;
        public readonly RootElement<TEvalContext, TExecContext> RootElement;

        protected readonly object lockObject = new object();

        private readonly System.Timers.Timer gestureTimeoutTimer = new System.Timers.Timer();

        internal readonly InvalidReleaseEventManager invalidReleaseEvents = new InvalidReleaseEventManager();

        public StrokeWatcher StrokeWatcher { get; internal set; }

        private IState currentState = null;
        public IState CurrentState
        {
            get => currentState;

            internal set
            {
                if (currentState != value)
                {
                    ResetStrokeWatcher();
                    if (value is State0<TConfig, TContextManager, TEvalContext, TExecContext>)
                    {
                        StopGestureTimeoutTimer();
                    }
                    else if (value is StateN<TConfig, TContextManager, TEvalContext, TExecContext>)
                    {
                        ResetGestureTimeoutTimer();
                    }
                    var lastState = currentState;
                    currentState = value;

                    OnStateChanged(new StateChangedEventArgs(lastState, currentState));
                }
            }
        }

        internal virtual TaskFactory StrokeWatcherTaskFactory => Task.Factory;
        internal virtual TaskFactory LowPriorityTaskFactory => Task.Factory;

        public GestureMachine(
            TConfig config,
            TContextManager contextManager,
            RootElement<TEvalContext, TExecContext> rootElement)
        {
            Config = config;
            ContextManager = contextManager;
            RootElement = rootElement;

            SetupGestureTimeoutTimer();

            CurrentState = new State0<TConfig, TContextManager, TEvalContext, TExecContext>(this, rootElement);
        }

        public virtual bool Input(IPhysicalEvent evnt) => Input(evnt, null);

        public virtual bool Input(IPhysicalEvent evnt, Point? point)
        {
            lock (lockObject)
            {
                if (point.HasValue && CurrentState is StateN<TConfig, TContextManager, TEvalContext, TExecContext>)
                {
                    StrokeWatcher.Queue(point.Value);
                }
                
                if (evnt is NullEvent)
                {
                    return false;
                }
                else if (evnt is ReleaseEvent releaseEvent && invalidReleaseEvents[releaseEvent] > 0)
                {
                    invalidReleaseEvents.CountDown(releaseEvent);
                    return true;
                }

                var (eventIsConsumed, nextState) = CurrentState.Input(evnt);
                CurrentState = nextState;
                return eventIsConsumed;
            }
        }

        private void SetupGestureTimeoutTimer()
        {
            gestureTimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(TryTimeout);
            gestureTimeoutTimer.Interval = Config.GestureTimeout;
            gestureTimeoutTimer.AutoReset = false;
        }

        private void StopGestureTimeoutTimer()
        {
            gestureTimeoutTimer.Stop();
        }

        private void ResetGestureTimeoutTimer()
        {
            gestureTimeoutTimer.Stop();
            gestureTimeoutTimer.Interval = Config.GestureTimeout;
            gestureTimeoutTimer.Start();
        }

        private void ReleaseGestureTimeoutTimer() => LazyRelease(gestureTimeoutTimer);

        private StrokeWatcher CreateStrokeWatcher()
            => new StrokeWatcher(
                StrokeWatcherTaskFactory,
                Config.StrokeStartThreshold,
                Config.StrokeDirectionChangeThreshold,
                Config.StrokeExtensionThreshold,
                Config.StrokeWatchInterval);

        private void ReleaseStrokeWatcher() => LazyRelease(StrokeWatcher);

        private void LazyRelease(IDisposable disposable)
        {
            if (disposable != null)
            {
                LowPriorityTaskFactory.StartNew(() => {
                    disposable.Dispose();
                });
            }
        }

        private void ResetStrokeWatcher()
        {
            var strokeWatcher = StrokeWatcher;
            StrokeWatcher = CreateStrokeWatcher();
            LazyRelease(strokeWatcher);
        }

        private void TryTimeout(object sender, System.Timers.ElapsedEventArgs args)
        {
            lock (lockObject)
            {
                if (CurrentState is StateN<TConfig, TContextManager, TEvalContext, TExecContext> lastState)
                {
                    var state = CurrentState;
                    var _state = CurrentState.Timeout();
                    while (state != _state)
                    {
                        state = _state;
                        _state = state.Timeout();
                    }
                    if (CurrentState != state)
                    {
                        CurrentState = state;
                        OnGestureTimeout(new GestureTimeoutEventArgs(lastState));
                    }
                }
            }
        }

        public void Reset()
        {
            lock (lockObject)
            {
                var lastState = CurrentState;
                if (CurrentState is StateN<TConfig, TContextManager, TEvalContext, TExecContext>)
                {
                    var state = CurrentState;
                    var _state = CurrentState.Reset();
                    while (state != _state)
                    {
                        state = _state;
                        _state = state.Reset();
                    }
                    CurrentState = state;
                }
                OnMachineReset(new MachineResetEventArgs(lastState));
            }
        }

        // StateChanged
        public class StateChangedEventArgs : EventArgs
        {
            public readonly IState LastState;
            public readonly IState CurrentState;

            public StateChangedEventArgs(IState lastState, IState currentState)
            {
                this.LastState = lastState;
                this.CurrentState = currentState;
            }
        }

        public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);

        public event StateChangedEventHandler StateChanged;

        internal virtual void OnStateChanged(StateChangedEventArgs e) => StateChanged?.Invoke(this, e);

        // StrokeReset
        // StrokeUpdated

        // GestureCancelled
        public class GestureCancelledEventArgs : EventArgs
        {
            public readonly StateN<TConfig, TContextManager, TEvalContext, TExecContext> LastState;

            public GestureCancelledEventArgs(StateN<TConfig, TContextManager, TEvalContext, TExecContext> stateN)
            {
                this.LastState = stateN;
            }
        }

        public delegate void GestureCancelledEventHandler(object sender, GestureCancelledEventArgs e);

        public event GestureCancelledEventHandler GestureCancelled;

        internal virtual void OnGestureCancelled(GestureCancelledEventArgs e) => GestureCancelled?.Invoke(this, e);

        // GestureTimeout
        public class GestureTimeoutEventArgs : EventArgs
        {
            public readonly StateN<TConfig, TContextManager, TEvalContext, TExecContext> LastState;

            public GestureTimeoutEventArgs(StateN<TConfig, TContextManager, TEvalContext, TExecContext> stateN)
            {
                this.LastState = stateN;
            }
        }

        public delegate void GestureTimeoutEventHandler(object sender, GestureTimeoutEventArgs e);

        public event GestureTimeoutEventHandler GestureTimeout;

        internal virtual void OnGestureTimeout(GestureTimeoutEventArgs e) => GestureTimeout?.Invoke(this, e);

        // MachineReset
        public class MachineResetEventArgs : EventArgs
        {
            public readonly IState LastState;

            public MachineResetEventArgs(IState states)
            {
                this.LastState = states;
            }
        }

        public delegate void MachineResetEventHandler(object sender, MachineResetEventArgs e);

        public event MachineResetEventHandler MachineReset;

        internal virtual void OnMachineReset(MachineResetEventArgs e) => MachineReset?.Invoke(this, e);

        public bool IsDisposed { get; private set; } = false;

        public void Dispose()
        {
            lock (lockObject)
            {
                GC.SuppressFinalize(this);
                IsDisposed = true;
                ReleaseGestureTimeoutTimer();
                ReleaseStrokeWatcher();
            }
        }

        ~GestureMachine()
        {
            Dispose();
        }
    }

    public class InvalidReleaseEventManager
    {
        public class NaturalNumberCounter<T>
        {
            private readonly Dictionary<T, int> Dictionary = new Dictionary<T, int>();

            public int this[T key]
            {
                get
                {
                    return Dictionary.TryGetValue(key, out int count) ? count : 0;
                }
                set
                {
                    if (value < 0)
                    {
                        throw new InvalidOperationException("n >= 0");
                    }
                    Dictionary[key] = value;
                }
            }

            public void CountDown(T key)
            {
                Dictionary[key] = this[key] - 1;
            }

            public void CountUp(T key)
            {
                Dictionary[key] = this[key] + 1;
            }
        }

        private readonly NaturalNumberCounter<ReleaseEvent> InvalidReleaseEvents = new NaturalNumberCounter<ReleaseEvent>();

        public int this[ReleaseEvent key]
        {
            get => InvalidReleaseEvents[key];
        }

        public void IgnoreNext(ReleaseEvent releaseEvent) => InvalidReleaseEvents.CountUp(releaseEvent);

        public void IgnoreNext(IEnumerable<ReleaseEvent> releaseEvents)
        {
            foreach (var releaseEvent in releaseEvents)
            {
                IgnoreNext(releaseEvent);
            }
        }

        public void CountDown(ReleaseEvent key) => InvalidReleaseEvents.CountDown(key);
    }
}
