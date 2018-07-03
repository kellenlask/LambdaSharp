namespace Pipe
{
    public static class PipeExtensions
    {
        public static Pipe<T> ToPipe<T>(this T value) => new Pipe<T>(value);


        public static EagerPipe<T> ToEagerPipe<T>(this T value) => new EagerPipe<T>(value);
    }
}
