using System;

namespace Pipe
{
    public class EagerPipe<T>
    {
        public T Result { get; }


        public EagerPipe(T result) => Result = result;


        public EagerPipe<TReturn> Into<TReturn>(Func<T, TReturn> fn) => new EagerPipe<TReturn>(fn(Result));


        public Pipe<T> ToLazy() => new Pipe<T>(Result);
    }
}
