using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Memoize
{
    public static class Memoization
    {
        /// <summary>
        ///     Memoizes the given Pure function so that it's not run multiple times per input; rather it looks up 
        ///     previously-encountered inputs in an internal table and returns them from the table if they exist there.
        /// <code>
        /// var calcFactorialMemoized = Memoize(calcFactorial); // Caches calculations so they don't have to be repeated
        /// </code>
        /// </summary>
        /// <param name="mappingFunc">The function that needs to be memoized</param>
        /// <typeparam name="TInput">The singular input type the memoized function accepts</typeparam>
        /// <typeparam name="TOutput">The output type the memoized function produces</typeparam>
        /// <returns>A caching function wrapping the function to be memoized.</returns>
        [Pure]
        public static Func<TInput, TOutput> Memoize<TInput, TOutput>(Func<TInput, TOutput> mappingFunc)
        {
            var cache = new Dictionary<TInput, TOutput>();

            return input => {
                if (!cache.ContainsKey(input))
                {
                    cache[input] = mappingFunc(input);
                }

                return cache[input];
            };
        }
    }
}
