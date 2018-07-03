using System;
using System.Linq;
using Combinatorics;
using Xunit;

namespace CombinatoricsTests
{
    public class ProductsExampleBased
    {
        [Fact]
        public void CartesianProductShouldCombine()
        {
            var results = new[] {"a", "b", "c"}.CartesianProduct(new[] {1, 2, 3}).ToArray();

            Assert.Equal(
                results,
                new[] {
                    ("a", 1), ("a", 2), ("a", 3),
                    ("b", 1), ("b", 2), ("b", 3),
                    ("c", 1), ("c", 2), ("c", 3),
                }
            );
        }


        [Fact]
        public void CartesianProductShouldCombineAndMap()
        {
            var results = new[] {"a", "b", "c"}.CartesianProduct(
                    new[] {1, 2, 3},
                    (a, b) => a + b
                )
                .ToArray();

            Assert.Equal(
                results,
                new[] {
                    "a1", "a2", "a3",
                    "b1", "b2", "b3",
                    "c1", "c2", "c3",
                }
            );
        }
    }
}
