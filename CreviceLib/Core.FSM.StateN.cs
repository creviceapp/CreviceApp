﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Crevice.Core.FSM
{
    using System.Linq;
    using Crevice.Core.Events;
    using Crevice.Core.Context;
    using Crevice.Core.DSL;
    using Crevice.Core.Stroke;

    public class StateN<TConfig, TContextManager, TEvalContext, TExecContext> : State
        where TConfig : GestureMachineConfig
        where TContextManager : ContextManager<TEvalContext, TExecContext>
        where TEvalContext : EvaluationContext
        where TExecContext : ExecutionContext
    {
        public readonly GestureMachine<TConfig, TContextManager, TEvalContext, TExecContext> Machine;

        public readonly TEvalContext Ctx;
        public readonly IReadOnlyList<(PhysicalReleaseEvent, IState)> History;
        public readonly IReadOnlyList<DoubleThrowElement<TExecContext>> DoubleThrowElements;
        public readonly bool CanCancel;
        
        public readonly IReadOnlyCollection<FireEvent> SingleThrowTriggers;
        public bool IsSingleThrowTrigger(PhysicalFireEvent fireEvent)
            => SingleThrowTriggers.Contains(fireEvent) ||
               SingleThrowTriggers.Contains(fireEvent.LogicalNormalized);

        public readonly IReadOnlyCollection<PressEvent> DoubleThrowTriggers;
        public bool IsDoubleThrowTrigger(PhysicalPressEvent pressEvent)
            => DoubleThrowTriggers.Contains(pressEvent) ||
               DoubleThrowTriggers.Contains(pressEvent.LogicalNormalized);

        public readonly IReadOnlyCollection<PhysicalReleaseEvent> EndTriggers;
        public bool IsEndTrigger(PhysicalReleaseEvent releaseEvent)
            => EndTriggers.Contains(releaseEvent);

        public readonly IReadOnlyCollection<PhysicalReleaseEvent> AbnormalEndTriggers;
        public bool IsAbnormalEndTrigger(PhysicalReleaseEvent releaseEvent)
            => AbnormalEndTriggers.Contains(releaseEvent);

        public StateN(
            GestureMachine<TConfig, TContextManager, TEvalContext, TExecContext> machine,
            TEvalContext ctx,
            IReadOnlyList<(PhysicalReleaseEvent, IState)> history,
            IReadOnlyList<DoubleThrowElement<TExecContext>> doubleThrowElements,
            int depth,
            bool canCancel = true)
            : base(depth)
        {
            Machine = machine;
            Ctx = ctx;
            History = history;
            DoubleThrowElements = doubleThrowElements;
            CanCancel = canCancel;

            // Caches.
            SingleThrowTriggers = GetSingleThrowTriggers(DoubleThrowElements);
            DoubleThrowTriggers = GetDoubleThrowTriggers(DoubleThrowElements);
            EndTriggers = GetEndTriggers(History);
            AbnormalEndTriggers = GetAbnormalEndTriggers(History);
        }

        public override (bool EventIsConsumed, IState NextState) Input(IPhysicalEvent evnt)
        {
            if (evnt is PhysicalFireEvent fireEvent && IsSingleThrowTrigger(fireEvent))
            {
                var singleThrowElements = GetSingleThrowElements(fireEvent);
                if (singleThrowElements.Any())
                {
                    Machine.ContextManager.ExecuteDoExecutors(Ctx, singleThrowElements);
                    var notCancellableCopyState = new StateN<TConfig, TContextManager, TEvalContext, TExecContext>(
                        Machine,
                        Ctx,
                        History,
                        DoubleThrowElements,
                        depth: Depth,
                        canCancel: false);
                    return (EventIsConsumed: true, NextState: notCancellableCopyState);
                }
            }
            else if (evnt is PhysicalPressEvent pressEvent && IsDoubleThrowTrigger(pressEvent))
            {
                var doubleThrowElements = GetDoubleThrowElements(pressEvent);
                if (doubleThrowElements.Any())
                {
                    Machine.ContextManager.ExecutePressExecutors(Ctx, doubleThrowElements);

                    if (CanTransition(doubleThrowElements))
                    {
                        var nextState = new StateN<TConfig, TContextManager, TEvalContext, TExecContext>(
                            Machine,
                            Ctx,
                            CreateHistory(History, pressEvent, this),
                            doubleThrowElements,
                            depth: Depth + 1,
                            canCancel: CanCancel);
                        return (EventIsConsumed: true, NextState: nextState);
                    }
                    return (EventIsConsumed: true, NextState: this);
                }
            }
            else if (evnt is PhysicalReleaseEvent releaseEvent)
            {
                if (IsNormalEndTrigger(releaseEvent))
                {
                    var strokes = Machine.StrokeWatcher.GetStorkes();
                    if (strokes.Any())
                    {
                        var strokeElements = GetStrokeElements(strokes);
                        Machine.ContextManager.ExecuteDoExecutors(Ctx, strokeElements);
                        Machine.ContextManager.ExecuteReleaseExecutors(Ctx, DoubleThrowElements);
                    }
                    else if (HasPressExecutors || HasDoExecutors || HasReleaseExecutors)
                    {
                        Machine.ContextManager.ExecuteDoExecutors(Ctx, DoubleThrowElements);
                        Machine.ContextManager.ExecuteReleaseExecutors(Ctx, DoubleThrowElements);
                    }
                    else if (CanCancel)
                    {
                        Machine.CallbackManager.OnGestureCancelled(this);
                    }
                    return (EventIsConsumed: true, NextState: LastState);
                }
                else if (IsAbnormalEndTrigger(releaseEvent))
                {
                    var (pastState, skippedReleaseEvents) = FindStateFromHistory(releaseEvent);
                    Machine.invalidEvents.IgnoreNext(skippedReleaseEvents);
                    return (EventIsConsumed: true, NextState: pastState);
                }
                else if (IsDoubleThrowTrigger(releaseEvent.Opposition))
                {
                    var doubleThrowElements = GetDoubleThrowElements(releaseEvent.Opposition);

                    // The following condition, will be true when the opposition of the DoubleThrowTrigger 
                    // of next `StateN` is given as a input, and when next `StateN` has press or release executors.
                    if (HasPressExecutors(doubleThrowElements) ||
                        HasReleaseExecutors(doubleThrowElements))
                    {
                        // If the next provisional `StateN` does not have any do executor, this `StateN` does not transit 
                        // to the next `StateN`, then release executers of it should be executed here.
                        // And if the release event comes firstly inverse to the expectation, in this pattern, 
                        // it should also be executed.
                        Machine.ContextManager.ExecuteReleaseExecutors(Ctx, doubleThrowElements);
                        return (EventIsConsumed: true, NextState: this);
                    }
                }
            }
            return base.Input(evnt);
        }

        public override IState Timeout()
        {
            if (!HasPressExecutors && !HasDoExecutors && !HasReleaseExecutors && CanCancel)
            {
                return LastState;
            }
            return this;
        }

        public override IState Reset()
        {
            Machine.invalidEvents.IgnoreNext(NormalEndTrigger);
            Machine.ContextManager.ExecuteReleaseExecutors(Ctx, DoubleThrowElements);
            return LastState;
        }

        public PhysicalReleaseEvent NormalEndTrigger => History.Last().Item1;

        public bool IsNormalEndTrigger(PhysicalReleaseEvent releaseEvent)
            => NormalEndTrigger == releaseEvent;

        public IState LastState => History.Last().Item2;

        public bool HasPressExecutors => HasPressExecutors(DoubleThrowElements);

        public bool HasDoExecutors => HasDoExecutors(DoubleThrowElements);

        public bool HasReleaseExecutors => HasReleaseExecutors(DoubleThrowElements);

        public IReadOnlyList<(PhysicalReleaseEvent, IState)> CreateHistory(
            IReadOnlyList<(PhysicalReleaseEvent, IState)> history,
            PhysicalPressEvent pressEvent,
            IState state)
        {
            var newHistory = history.ToList();
            newHistory.Add((pressEvent.Opposition, state));
            return newHistory;
        }

        public static IReadOnlyCollection<PhysicalReleaseEvent> GetEndTriggers(IReadOnlyList<(PhysicalReleaseEvent, IState)> history)
            => new HashSet<PhysicalReleaseEvent>(from h in history select h.Item1);

        public IReadOnlyCollection<PhysicalReleaseEvent> GetAbnormalEndTriggers(IReadOnlyList<(PhysicalReleaseEvent, IState)> history)
            => new HashSet<PhysicalReleaseEvent>(from h in history.Reverse().Skip(1) select h.Item1);

        public (IState, IReadOnlyList<PhysicalReleaseEvent>) FindStateFromHistory(PhysicalReleaseEvent releaseEvent)
        {
            var nextHistory = History.TakeWhile(t => t.Item1 != releaseEvent);
            var foundState = History[nextHistory.Count()].Item2;
            var skippedReleaseEvents = History.Skip(nextHistory.Count()).Select(t => t.Item1).ToList();
            return (foundState, skippedReleaseEvents);
        }

        public IReadOnlyList<DoubleThrowElement<TExecContext>> GetDoubleThrowElements(PhysicalPressEvent triggerEvent)
            => (from d in DoubleThrowElements
                where d.IsFull
                select (
                    from dd in d.DoubleThrowElements
                    where dd.IsFull && (dd.Trigger.Equals(triggerEvent) ||
                                        dd.Trigger.Equals(triggerEvent.LogicalNormalized))
                    select dd))
                .Aggregate(new List<DoubleThrowElement<TExecContext>>(), (a, b) => { a.AddRange(b); return a; });

        public IReadOnlyList<StrokeElement<TExecContext>> GetStrokeElements(IReadOnlyList<StrokeDirection> strokes)
            => (from d in DoubleThrowElements
                where d.IsFull
                select (
                    from ds in d.StrokeElements
                    where ds.IsFull && ds.Strokes.SequenceEqual(strokes)
                    select ds))
                .Aggregate(new List<StrokeElement<TExecContext>>(), (a, b) => { a.AddRange(b); return a; });

        public IReadOnlyList<SingleThrowElement<TExecContext>> GetSingleThrowElements(PhysicalFireEvent triggerEvent)
            => (from d in DoubleThrowElements
                where d.IsFull
                select (
                    from ds in d.SingleThrowElements
                    where ds.IsFull && (ds.Trigger.Equals(triggerEvent) ||
                                       ds.Trigger.Equals(triggerEvent.LogicalNormalized))
                    select ds))
                .Aggregate(new List<SingleThrowElement<TExecContext>>(), (a, b) => { a.AddRange(b); return a; });
        
        public static IReadOnlyCollection<FireEvent> GetSingleThrowTriggers(
            IReadOnlyList<DoubleThrowElement<TExecContext>> doubleThrowElements)
            => (from d in doubleThrowElements
                where d.IsFull
                select (
                    from ds in d.SingleThrowElements
                    where ds.IsFull
                    select ds.Trigger))
                .Aggregate(new HashSet<FireEvent>(), (a, b) => { a.UnionWith(b); return a; });

        public static IReadOnlyCollection<PressEvent> GetDoubleThrowTriggers(
            IReadOnlyList<DoubleThrowElement<TExecContext>> doubleThrowElements)
            => (from ds in doubleThrowElements
                where ds.IsFull
                select (
                    from dd in ds.DoubleThrowElements
                    where dd.IsFull
                    select dd.Trigger))
                .Aggregate(new HashSet<PressEvent>(), (a, b) => { a.UnionWith(b); return a; });
    }
}