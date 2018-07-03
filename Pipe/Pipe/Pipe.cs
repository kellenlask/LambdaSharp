using System;

namespace Pipe
{
    public class Pipe<T>
    {
        public Func<T> Result { get; }


        public Pipe(T result) => Result = () => result;


        public Pipe(Func<T> resultFunc) => Result = resultFunc;


        public Pipe<TReturn> Into<TReturn>(Func<T, TReturn> fn) => new Pipe<TReturn>(() => fn(Result()));
    }
}