using System.Linq;

namespace In
{
    public static class InExtensions
    {
        /// <summary>
        ///     Returns whether or not the given value is in the passed collection of values.
        ///     <code>
        ///         if(user.Status.In(Status.Inactive, Status.Disabled, Status.Blacklisted)) {
        ///             ...
        ///         }
        ///     </code>
        /// </summary>
        /// <param name="value">The item whose membership you want to test</param>
        /// <param name="options">The set of items from which membership is determined</param>
        /// <returns>True/False for membership in the passed set</returns>
        public static bool In<T>(this T value, params T[] options) => options.Contains(value);
    }
}
