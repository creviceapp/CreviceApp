﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Crevice.Core.DSL
{
    using System.Linq;
    using Crevice.Core.Events;
    using Crevice.Core.Context;
    using Crevice.Core.Stroke;

    public abstract class Element
    {
        public abstract bool IsFull { get; }
    }

    /* RootElement
     * 
     * .When() -> new WhenElement
     */
    public class RootElement<TEvalContext, TExecContext> : Element
        where TEvalContext : EvaluationContext
        where TExecContext : ExecutionContext
    {
        public override bool IsFull => WhenElements.Any(e => e.IsFull);

        private readonly List<WhenElement<TEvalContext, TExecContext>> whenElements = new List<WhenElement<TEvalContext, TExecContext>>();
        public IReadOnlyList<WhenElement<TEvalContext, TExecContext>> WhenElements => whenElements.ToList();

        public WhenElement<TEvalContext, TExecContext> When(EvaluateAction<TEvalContext> evaluator)
        {
            var elm = new WhenElement<TEvalContext, TExecContext>(evaluator);
            whenElements.Add(elm);
            return elm;
        }
    }

    /* WhenElement
     * 
     * .On(FireEvent) -> new SingleThrowElement
     * 
     * .On(PressEvent) -> new DoubleThrowElement
     */
    public class WhenElement<TEvalContext, TExecContext> : Element
        where TEvalContext : EvaluationContext
        where TExecContext : ExecutionContext
    {
        public override bool IsFull
            => WhenEvaluator != null &&
                (SingleThrowElements.Any(e => e.IsFull) ||
                 DoubleThrowElements.Any(e => e.IsFull));

        public readonly EvaluateAction<TEvalContext> WhenEvaluator;

        private readonly List<SingleThrowElement<TExecContext>> singleThrowElements = new List<SingleThrowElement<TExecContext>>();
        public IReadOnlyList<SingleThrowElement<TExecContext>> SingleThrowElements => singleThrowElements.ToList();

        private readonly List<DoubleThrowElement<TExecContext>> doubleThrowElements = new List<DoubleThrowElement<TExecContext>>();
        public IReadOnlyList<DoubleThrowElement<TExecContext>> DoubleThrowElements => doubleThrowElements.ToList();

        public WhenElement(EvaluateAction<TEvalContext> evaluator)
        {
            WhenEvaluator = evaluator;
        }

        public SingleThrowElement<TExecContext> On(FireEvent triggerEvent)
        {
            var elm = new SingleThrowElement<TExecContext>(triggerEvent);
            singleThrowElements.Add(elm);
            return elm;
        }

        public DoubleThrowElement<TExecContext> On(PressEvent triggerEvent)
        {
            var elm = new DoubleThrowElement<TExecContext>(triggerEvent);
            doubleThrowElements.Add(elm);
            return elm;
        }
    }

    /* SingleThrowElement
     * 
     * .Do() -> this
     */
    public class SingleThrowElement<T> : Element
        where T : ExecutionContext
    {
        public override bool IsFull => Trigger != null && DoExecutors.Any(e => e != null);

        public readonly FireEvent Trigger;

        private readonly List<ExecuteAction<T>> doExecutors = new List<ExecuteAction<T>>();
        public IReadOnlyList<ExecuteAction<T>> DoExecutors => doExecutors.ToList();

        public SingleThrowElement(FireEvent triggerEvent)
        {
            Trigger = triggerEvent;
        }

        public SingleThrowElement<T> Do(ExecuteAction<T> executor)
        {
            doExecutors.Add(executor);
            return this;
        }
    }

    /* 
     * .Press() -> this 
     * 
     * .Do() -> this 
     * 
     * .Release() -> this 
     * 
     * .On(FireEvent) -> new SingleThrowElement
     * 
     * .On(PressEvent) -> new DoubleThrowElement
     * 
     * .On(StrokeEvent) -> new StrokeEelement
     */
    public class DoubleThrowElement<T> : Element
        where T : ExecutionContext
    {
        public override bool IsFull
            => Trigger != null &&
                (PressExecutors.Any(e => e != null) ||
                 DoExecutors.Any(e => e != null) ||
                 ReleaseExecutors.Any(e => e != null) ||
                 SingleThrowElements.Any(e => e.IsFull) ||
                 DoubleThrowElements.Any(e => e.IsFull) ||
                 StrokeElements.Any(e => e.IsFull));

        public readonly PressEvent Trigger;

        private readonly List<SingleThrowElement<T>> singleThrowElements = new List<SingleThrowElement<T>>();
        public IReadOnlyList<SingleThrowElement<T>> SingleThrowElements => singleThrowElements.ToList();

        private readonly List<DoubleThrowElement<T>> doubleThrowElements = new List<DoubleThrowElement<T>>();
        public IReadOnlyList<DoubleThrowElement<T>> DoubleThrowElements => doubleThrowElements.ToList();

        private readonly List<StrokeElement<T>> strokeElements = new List<StrokeElement<T>>();
        public IReadOnlyList<StrokeElement<T>> StrokeElements => strokeElements.ToList();

        private readonly List<ExecuteAction<T>> pressExecutors = new List<ExecuteAction<T>>();
        public IReadOnlyList<ExecuteAction<T>> PressExecutors => pressExecutors.ToList();

        private readonly List<ExecuteAction<T>> doExecutors = new List<ExecuteAction<T>>();
        public IReadOnlyList<ExecuteAction<T>> DoExecutors => doExecutors.ToList();

        private readonly List<ExecuteAction<T>> releaseExecutors = new List<ExecuteAction<T>>();
        public IReadOnlyList<ExecuteAction<T>> ReleaseExecutors => releaseExecutors.ToList();

        public DoubleThrowElement(PressEvent triggerEvent)
        {
            Trigger = triggerEvent;
        }

        public SingleThrowElement<T> On(FireEvent triggerEvent)
        {
            var elm = new SingleThrowElement<T>(triggerEvent);
            singleThrowElements.Add(elm);
            return elm;
        }

        public DoubleThrowElement<T> On(PressEvent triggerEvent)
        {
            var elm = new DoubleThrowElement<T>(triggerEvent);
            doubleThrowElements.Add(elm);
            return elm;
        }

        public StrokeElement<T> On(params StrokeDirection[] strokeDirections)
        {
            var elm = new StrokeElement<T>(strokeDirections);
            strokeElements.Add(elm);
            return elm;
        }

        public DoubleThrowElement<T> Press(ExecuteAction<T> executor)
        {
            pressExecutors.Add(executor);
            return this;
        }

        public DoubleThrowElement<T> Do(ExecuteAction<T> executor)
        {
            doExecutors.Add(executor);
            return this;
        }

        public DoubleThrowElement<T> Release(ExecuteAction<T> executor)
        {
            releaseExecutors.Add(executor);
            return this;
        }
    }

    /* 
     * .Do() -> this 
     */
    public class StrokeElement<T> : Element
        where T : ExecutionContext
    {
        public override bool IsFull => Strokes.Any() && DoExecutors.Any(e => e != null);

        public readonly IReadOnlyList<StrokeDirection> Strokes;

        private readonly List<ExecuteAction<T>> doExecutors = new List<ExecuteAction<T>>();
        public IReadOnlyList<ExecuteAction<T>> DoExecutors => doExecutors.ToList();

        public StrokeElement(params StrokeDirection[] strokes)
        {
            Strokes = strokes;
        }

        public StrokeElement<T> Do(ExecuteAction<T> executor)
        {
            doExecutors.Add(executor);
            return this;
        }
    }
}
