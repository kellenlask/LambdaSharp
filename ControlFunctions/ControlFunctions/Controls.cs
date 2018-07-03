using System;
using System.Diagnostics.Contracts;

namespace ControlFunctions
{
    /// <summary>
    ///     TODO: 
    ///     Methods imported from the old version of LambdaSharp that I was working on... Unsure how I feel about them
    ///     so far... I think I might want to name them to mirror their C# counterpart control structures... 
    /// </summary>
    public static class Controls
    {
        /// <summary>
        ///    Imagine 'if' were a function...
        /// </summary>
        /// <param name="condition">The condition to evaluate to determine which branch to take</param>
        /// <param name="trueBranch">The function to execute if the predicate evaluates to true</param>
        /// <param name="falseBranch">The function to execute if the predicate evaluates to false</param>
        [Pure]
        public static Action Branch(bool condition, Action trueBranch = null, Action falseBranch = null) => condition
            ? new Action(() => trueBranch?.Invoke())
            : (() => falseBranch?.Invoke());


        /// <summary>
        ///    Imagine 'if' were a function...
        /// </summary>
        /// <param name="predicate">The condition to evaluate to determine which branch to take</param>
        /// <param name="trueBranch">The function to execute if the predicate evaluates to true</param>
        /// <param name="falseBranch">The function to execute if the predicate evaluates to false</param>
        [Pure]
        public static Action Branch(Func<bool> predicate, Action trueBranch = null, Action falseBranch = null) =>
            Branch(predicate?.Invoke() ?? false, trueBranch, falseBranch);


        /// <summary>
        ///    Imagine 'if' were a function...
        /// </summary>
        /// <param name="condition">The condition to evaluate to determine which branch to take</param>
        /// <param name="trueBranch">The function to execute if the predicate evaluates to true</param>
        /// <param name="falseBranch">The function to execute if the predicate evaluates to false</param>
        public static void DoBranch(bool condition, Action trueBranch = null, Action falseBranch = null) =>
            Branch(condition, trueBranch, falseBranch).Invoke();


        /// <summary>
        ///    Imagine 'if' were a function...
        /// </summary>
        /// <param name="predicate">The condition to evaluate to determine which branch to take</param>
        /// <param name="trueBranch">The function to execute if the predicate evaluates to true</param>
        /// <param name="falseBranch">The function to execute if the predicate evaluates to false</param>
        public static void DoBranch(Func<bool> predicate, Action trueBranch = null, Action falseBranch = null) =>
            Branch(predicate, trueBranch, falseBranch).Invoke();


        /// <summary>
        ///    Imagine 'if' were a function...
        /// </summary>
        /// <param name="condition">The condition to evaluate to determine which branch to take</param>
        /// <param name="trueBranch">The function to execute if the predicate evaluates to true</param>
        /// <param name="falseBranch">The function to execute if the predicate evaluates to false</param>
        [Pure]
        public static T DoBranch<T>(bool condition, Func<T> trueBranch = null, Func<T> falseBranch = null) => condition
            ? (trueBranch == null ? default(T) : trueBranch())
            : (falseBranch == null ? default(T) : falseBranch());


        /// <summary>
        ///    Imagine 'if' were a function...
        /// </summary>
        /// <param name="condition">The condition to evaluate to determine which branch to take</param>
        /// <param name="trueBranch">The function to execute if the predicate evaluates to true</param>
        /// <param name="falseBranch">The function to execute if the predicate evaluates to false</param>
        [Pure]
        public static Func<T> Branch<T>(
            bool condition,
            Func<T> trueBranch = null,
            Func<T> falseBranch = null
        ) => condition
            ? (trueBranch ?? (() => default(T)))
            : (falseBranch ?? (() => default(T)));


        public static void While(Func<bool> predicate, Action toPerform)
        {
            while (predicate?.Invoke() ?? false)
            {
                toPerform?.Invoke();
            }
        }


        [Pure]
        public static TOut TryCatch<TOut, TExcept>(
            Func<TOut> toTry,
            Func<TExcept, TOut> exceptionHandler
        ) where TExcept : Exception
        {
            try
            {
                return toTry();
            }
            catch (TExcept ex)
            {
                return exceptionHandler(ex);
            }
        }


        [Pure]
        public static T TryCatch<T>(
            Func<T> toTry,
            Func<Exception, T> exceptionHandler
        ) => TryCatch<T, Exception>(toTry, exceptionHandler);


        public static void TryCatch<T>(Action toTry, Action<T> exceptionHandler) where T : Exception
        {
            var result = TryCatch<bool, T>(
                () => {
                    toTry();
                    return true;
                },
                ex => {
                    exceptionHandler(ex);
                    return false;
                }
            );
        }


        public static void TryCatch(
            Action toTry,
            Action<Exception> exceptionHandler
        ) => TryCatch<Exception>(toTry, exceptionHandler);
    }
}
