using System;
using System.Collections.Generic;
using System.Linq;

namespace Combinatorics
{
    public static class Products
    {
        public static IEnumerable<(TOne, TTwo)> CartesianProduct<TOne, TTwo>(
            this IEnumerable<TOne> one,
            IEnumerable<TTwo> two
        )
        {
            var twoArray = two.ToArray(); // Avoid Multiple Enumeration

            foreach (var elementOne in one)
            foreach (var elementTwo in twoArray)
            {
                yield return (elementOne, elementTwo);
            }
        }


        public static IEnumerable<(TOne, TTwo, TThree)> CartesianProduct<TOne, TTwo, TThree>(
            this IEnumerable<TOne> one,
            IEnumerable<TTwo> two,
            IEnumerable<TThree> three
        ) => one.CartesianProduct(two.CartesianProduct(three), (first, second) => (first, second.Item1, second.Item2));


        public static IEnumerable<TThree> CartesianProduct<TOne, TTwo, TThree>(
            this IEnumerable<TOne> one,
            IEnumerable<TTwo> two,
            Func<TOne, TTwo, TThree> mapFunc
        )
        {
            var twoArray = two.ToArray(); // Avoid Multiple Enumeration

            foreach (var elementOne in one)
            foreach (var elementTwo in twoArray)
            {
                yield return mapFunc(elementOne, elementTwo);
            }
        }


        public static IEnumerable<TFour> CartesianProduct<TOne, TTwo, TThree, TFour>(
            this IEnumerable<TOne> one,
            IEnumerable<TTwo> two,
            IEnumerable<TThree> three,
            Func<TOne, TTwo, TThree, TFour> mapFunc
        ) => one.CartesianProduct(two.CartesianProduct(three), (first, second) => (first, second.Item1, second.Item2))
            .Select(combos => mapFunc(combos.Item1, combos.Item2, combos.Item3));
    }
}
