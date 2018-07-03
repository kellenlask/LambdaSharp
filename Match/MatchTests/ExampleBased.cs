using Match;
using Xunit;

namespace MatchTests
{
    public class ExampleBased
    {
        [Fact]
        public void ItShouldMatch()
        {
            var matcher = new Match<string, string>()
                .When(s => s.Length == 0, "zero")
                .When(s => s.Length == 1, "one")
                .When(s => s.Length == 2, "two")
                .When(s => s.Length == 3, "three")
                .When(s => s.Length == 4, "four")
                .When(s => s.Length == 5, "five")
                .Otherwise(s => s.Length.ToString());

            Assert.Equal(matcher.For(""), "zero");
            Assert.Equal(matcher.For("1"), "one");
            Assert.Equal(matcher.For("22"), "two");
            Assert.Equal(matcher.For("333"), "three");
            Assert.Equal(matcher.For("4444"), "four");
            Assert.Equal(matcher.For("55555"), "five");
            Assert.Equal(matcher.For("666666"), "6");
        }
    }
}
