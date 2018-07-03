using Pipe;
using Xunit;

namespace PipeTests
{
    public class ExampleBased
    {
        [Fact]
        public void ItShouldPipe()
        {
            var result = new Pipe<string>("foo")
                .Into(s => s.Length)
                .Into(i => i.ToString())
                .Result();

            Assert.Equal(result, "3");
        }


        [Fact]
        public void ItShouldEagerPipe()
        {
            var result = new EagerPipe<string>("foo")
                .Into(s => s.Length)
                .Into(i => i.ToString())
                .Result;

            Assert.Equal(result, "3");
        }


        [Fact]
        public void ItShouldBeLazy()
        {
            var calledFuncOne = false;
            var calledFuncTwo = false;
            var calledFuncThree = false;


            var running = new Pipe<string>("foo");

            Assert.False(calledFuncOne || calledFuncTwo || calledFuncThree);

            var runningOne = running.Into(
                s => {
                    calledFuncOne = true;
                    return s.Length;
                }
            );

            Assert.False(calledFuncOne || calledFuncTwo || calledFuncThree);

            var runningTwo = runningOne.Into(
                i => {
                    calledFuncTwo = true;
                    return i.ToString();
                }
            );

            Assert.False(calledFuncOne || calledFuncTwo || calledFuncThree);

            var runningThree = runningTwo.Into(
                s => {
                    calledFuncThree = true;
                    return s + "Hi";
                }
            );

            Assert.False(calledFuncOne || calledFuncThree || calledFuncThree);

            var result = runningThree.Result();

            Assert.Equal(result, "3Hi");
            Assert.True(calledFuncOne && calledFuncTwo && calledFuncThree);
        }


        [Fact]
        public void ItShouldBeEager()
        {
            var calledFuncOne = false;
            var calledFuncTwo = false;
            var calledFuncThree = false;


            var running = new EagerPipe<string>("foo");

            Assert.False(calledFuncOne || calledFuncTwo || calledFuncThree);

            var runningOne = running.Into(
                s => {
                    calledFuncOne = true;
                    return s.Length;
                }
            );

            Assert.True(calledFuncOne);
            Assert.False(calledFuncTwo || calledFuncThree);

            var runningTwo = runningOne.Into(
                i => {
                    calledFuncTwo = true;
                    return i.ToString();
                }
            );

            Assert.True(calledFuncOne && calledFuncTwo);
            Assert.False(calledFuncThree);

            var runningThree = runningTwo.Into(
                s => {
                    calledFuncThree = true;
                    return s + "Hi";
                }
            );

            Assert.True(calledFuncOne && calledFuncThree && calledFuncThree);

            var result = runningThree.Result;

            Assert.Equal(result, "3Hi");
        }
    }
}
