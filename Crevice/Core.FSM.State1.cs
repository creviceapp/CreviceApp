﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crevice.Core.FSM
{
    public class State1 : State
    {
        internal readonly State0 S0;
        internal readonly State2 S2;
        internal readonly ActionContext ctx;
        internal readonly Def.Event.IDoubleActionSet primaryEvent;
        internal readonly IDictionary<Def.Event.ISingleAction, IEnumerable<OnButtonWithIfButtonGestureDefinition>> T0;
        internal readonly IDictionary<Def.Event.IDoubleActionSet, IEnumerable<OnButtonWithIfButtonGestureDefinition>> T1;
        internal readonly IDictionary<Def.Stroke, IEnumerable<OnButtonWithIfStrokeGestureDefinition>> T2;
        internal readonly IEnumerable<IfButtonGestureDefinition> T3;

        //todo
        //private readonly SingleInputSender InputSender = new SingleInputSender();
        
        public State1(
            StateGlobal Global,
            State0 S0,
            ActionContext ctx,
            Def.Event.IDoubleActionSet primaryEvent,
            IEnumerable<OnButtonGestureDefinition> T1,
            IEnumerable<IfButtonGestureDefinition> T2
            ) : base(Global)
        {
            this.S0 = S0;
            this.ctx = ctx;
            this.primaryEvent = primaryEvent;
            this.T0 = Transition.Gen1_0(T1);
            this.T1 = Transition.Gen1_1(T1);
            this.T2 = Transition.Gen1_2(T1);
            this.T3 = T2;
            this.S2 = new State2(Global, S0, ctx, primaryEvent, this.T0, this.T1, this.T2, this.T3);
        }

        public override Result Input(Def.Event.IEvent evnt, System.Drawing.Point point)
        {
            // Special side effect 3, 4
            if (MustBeIgnored(evnt))
            {
                return Result.EventIsConsumed(nextState: this);
            }
            // Special side effect 2
            Global.StrokeWatcher.Queue(point);

            if (evnt is Def.Event.ISingleAction)
            {
                var ev = evnt as Def.Event.ISingleAction;
                if (T0.Keys.Contains(ev))
                {
                    Verbose.Print("[Transition 1_0]");
                    ExecuteUserDoFuncInBackground(ctx, T0[ev]);
                    return Result.EventIsConsumed(nextState: S2);
                }
            }
            else if (evnt is Def.Event.IDoubleActionSet)
            {
                var ev = evnt as Def.Event.IDoubleActionSet;
                if (T1.Keys.Contains(ev))
                {
                    Verbose.Print("[Transition 1_1]");
                    ExecuteUserBeforeFuncInBackground(ctx, T1[ev]);
                    return Result.EventIsConsumed(nextState: new State3(Global, S0, S2, ctx, primaryEvent, ev, T3, T1[ev]));
                }
            }
            else if (evnt is Def.Event.IDoubleActionRelease)
            {
                var ev = evnt as Def.Event.IDoubleActionRelease;
                if (ev == primaryEvent.GetPair())
                {
                    var stroke = Global.StrokeWatcher.GetStorke();
                    if (stroke.Count() > 0)
                    {
                        Verbose.Print("Stroke: {0}", stroke.ToString());
                        if (T2.Keys.Contains(stroke))
                        {
                            Verbose.Print("[Transition 1_2]");
                            ExecuteUserDoFuncInBackground(ctx, T2[stroke]);
                            ExecuteUserAfterFuncInBackground(ctx, T3);
                        }
                    }
                    else
                    {
                        if (T3.Count() > 0)
                        {
                            Verbose.Print("[Transition 1_3]");
                            ExecuteUserDoFuncInBackground(ctx, T3);
                            ExecuteUserAfterFuncInBackground(ctx, T3);
                        }
                        else
                        {
                            Verbose.Print("[Transition 1_4]");
                            //todo
                            //ExecuteInBackground(ctx, RestorePrimaryButtonClickEvent());
                        }
                    }
                    return Result.EventIsConsumed(nextState: S0);
                }
            }
            return base.Input(evnt, point);
        }

        // TOdo: 

        // Todo: IsCancellable を共通インターフェイスにする or IsTimeoutable

        // Todo: んで、IsRestorable

        public IState Cancel()
        {
            if (!HasBeforeOrAfter) // Todo: IsCancelable がいいかな
            {
                // リストア可能でないなら、IgnoreNext、かな。IgnoreNextがデフォルト？ eventhanderかな

                // これはストロークの数が０であることも復元の条件（なのでポインタの原点を持っていなくてもうまくいっている）
                // ポインタの初期位置を復元するのもおかしいので難しいところだが、この実装が正しいと思う

                Verbose.Print("[Transition 1_5]");
                //todo
                //ExecuteInBackground(ctx, RestorePrimaryButtonDownEvent()); 
                return S0;
            }
            else
            {
                // キャンセルを無視するフロー
                return this;
            }
        }

        internal bool HasBeforeOrAfter
        {
            get
            {
                return T3
                    .Where(x => x.beforeFunc != null || x.afterFunc != null)
                    .Count() > 0;
            }
        }

        public override IState Reset()
        {
            Verbose.Print("[Transition 1_6]");
            IgnoreNext(primaryEvent.GetPair());
            ExecuteUserAfterFuncInBackground(ctx, T3);
            return S0;
        }

        /*
        internal Action RestorePrimaryButtonDownEvent()
        {
            return () =>
            {
                if (primaryEvent == Def.Constant.LeftButtonDown)
                {
                    InputSender.LeftDown();
                }
                else if (primaryEvent == Def.Constant.MiddleButtonDown)
                {
                    InputSender.MiddleDown();
                }
                else if (primaryEvent == Def.Constant.RightButtonDown)
                {
                    InputSender.RightDown();
                }
                else if (primaryEvent == Def.Constant.X1ButtonDown)
                {
                    InputSender.X1Down();
                }
                else if (primaryEvent == Def.Constant.X2ButtonDown)
                {
                    InputSender.X2Down();
                }
            };
        }
        */

        /*
        internal Action RestorePrimaryButtonClickEvent()
        {
            return () =>
            {
                if (primaryEvent == Def.Constant.LeftButtonDown)
                {
                    InputSender.LeftClick();
                }
                else if (primaryEvent == Def.Constant.MiddleButtonDown)
                {
                    InputSender.MiddleClick();
                }
                else if (primaryEvent == Def.Constant.RightButtonDown)
                {
                    InputSender.RightClick();
                }
                else if (primaryEvent == Def.Constant.X1ButtonDown)
                {
                    InputSender.X1Click();
                }
                else if (primaryEvent == Def.Constant.X2ButtonDown)
                {
                    InputSender.X2Click();
                }
            };
        }
        */
    }
}
