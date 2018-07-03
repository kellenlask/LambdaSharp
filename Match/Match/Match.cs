using System;
using System.Collections.Generic;
using System.Linq;

namespace Match
{
    public class Match<TIn, TOut>
    {
        private Func<TIn, TOut> _default = val => default(TOut);

        private readonly List<(Predicate<TIn>, Func<TIn, TOut>)> _cases = new List<(Predicate<TIn>, Func<TIn, TOut>)>();


        public Match<TIn, TOut> When(Predicate<TIn> predicate, Func<TIn, TOut> fn)
        {
            _cases.Add((predicate, fn));
            return this;
        }


        public Match<TIn, TOut> When(Predicate<TIn> predicate, TOut value) => When(predicate, val => value);


        public Match<TIn, TOut> When(bool condition, Func<TIn, TOut> value) => When(val => condition, value);


        public Match<TIn, TOut> When(bool condition, TOut value) => When(val => condition, val => value);


        public Match<TIn, TOut> Otherwise(Func<TIn, TOut> fn)
        {
            _default = fn;
            return this;
        }


        public Match<TIn, TOut> Otherwise(TOut defaultValue) => Otherwise(value => defaultValue);


        public TOut For(TIn value)
        {
            var foo = _cases.FirstOrDefault(nextCase => nextCase.Item1(value));
            return (foo.Item2 ?? _default)(value);
        }
    }
}
