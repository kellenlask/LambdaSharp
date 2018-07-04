using System;
using System.Collections.Generic;
using System.Linq;

namespace Match
{
    /// <summary>
    ///     Pattern matcher! Like a switch, if a switch was smaller, fluent, and understood predicates. While other
    ///     languages have far nice pattern matching, this is enormously better than C#'s natural abilities.
    /// <para/>
    /// <code>
    /// var matcher = new Match&lt;int, int&gt;()
    ///                   .when(number => number % 2 == 0, number => number / 2)
    ///                   .Otherwise(number => 3 * number + 1);
    ///
    /// var one = matcher.for(1); // 4
    /// var two = matcher.for(2); // 1
    /// </code>
    /// </summary>
    /// <typeparam name="TIn">The input type that will be matched against</typeparam>
    /// <typeparam name="TOut">The output type that will be evaluated to</typeparam>
    public class Match<TIn, TOut>
    {
        // The default function to evaluate if no other branch evaluates
        private Func<TIn, TOut> _default = toMatch => default(TOut);

        // List of cases in order of execution
        private readonly List<(Predicate<TIn>, Func<TIn, TOut>)> _cases = new List<(Predicate<TIn>, Func<TIn, TOut>)>();


        /// <summary>
        ///     Add a match branch that is taken when the given predicate evaluates true, and none of the preceeding
        ///     matches were positive. This branch will return the evaluation of the given function as applied to the
        ///     value being matched on. 
        /// </summary>
        /// <param name="predicate">whether or not to take this branch if it is reached</param>
        /// <param name="fn">The function from which to derive the return value when evaluating this branch</param>
        /// <returns>this</returns>
        public Match<TIn, TOut> When(Predicate<TIn> predicate, Func<TIn, TOut> fn)
        {
            _cases.Add((predicate, fn));
            return this;
        }


        /// <summary>
        ///     Add a match branch that is taken when the given predicate evaluates true, and none of the preceeding
        ///     matches were positive. This branch will return the given value when evaluated. 
        /// </summary>
        /// <param name="predicate">whether or not to take this branch if it is reached</param>
        /// <param name="value">The function from which to derive the return value when evaluating this branch</param>
        /// <returns>this</returns>
        public Match<TIn, TOut> When(Predicate<TIn> predicate, TOut value) => When(predicate, toMatch => value);


        /// <summary>
        ///     Add a match branch that is taken when the given boolean is true, and none of the preceeding matches were
        ///     positive. This branch will return the evaluation of the given function as applied to the value being
        ///     matched on. 
        /// </summary>
        /// <param name="condition">whether or not to take this branch if it is reached</param>
        /// <param name="fn">The function from which to derive the return value when evaluating this branch</param>
        /// <returns>this</returns>
        public Match<TIn, TOut> When(bool condition, Func<TIn, TOut> fn) => When(toMatch => condition, fn);


        /// <summary>
        ///     Add a match branch that is taken when the given boolean is true, and none of the preceeding matches were
        ///     positive. This branch will return the given value, regardless of match input.
        /// </summary>
        /// <param name="condition">whether or not to take this branch if it is reached</param>
        /// <param name="value">The value to return when evaluating the branch</param>
        /// <returns>this</returns>
        public Match<TIn, TOut> When(bool condition, TOut value) => When(toMatch => condition, toMatch => value);


        /// <summary>
        ///     Add a default branch that returns the result of the given function as applied to the value in question
        ///     at the time of matching if no other branches matched.
        /// </summary>
        /// <param name="fn">The function from which to derive the return value when no other branch is evaluated</param>
        /// <returns>this</returns>
        public Match<TIn, TOut> Otherwise(Func<TIn, TOut> fn)
        {
            _default = fn;
            return this;
        }


        /// <summary>
        ///     Add a default branch that returns the given value if no other branches matched on the value in question
        ///     at the time of matching.
        /// </summary>
        /// <returns>this</returns>
        public Match<TIn, TOut> Otherwise(TOut defaultValue) => Otherwise(toMatch => defaultValue);


        /// <summary>
        ///     Apply this pattern matcher to the given value. 
        /// </summary>
        /// <param name="toMatch">The value to match against</param>
        /// <returns>
        ///     The result of the first match found. Matches are evaluated in the order that they are added. If no match
        ///     is found, returns the Otherwise value. If no Otherwise was provided, returns default for the given type.
        /// </returns>
        public TOut For(
            TIn toMatch
        ) => (_cases.FirstOrDefault(nextCase => nextCase.Item1(toMatch)).Item2 ?? _default)(toMatch);
    }
}
