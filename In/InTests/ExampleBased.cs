using In;
using Xunit;

namespace InTests
{
    public class ExampleBased
    {
        [Fact]
        public void ItShouldDetectMembership()
        {
            var result = "foo".In("foo", "bar", "baz");
            Assert.True(result);
        }


        [Fact]
        public void ItShouldReturnFalseForEmptyList()
        {
            var result = "foo".In();
            Assert.False(result);
        }
    }
}
