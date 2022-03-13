using System;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    /// <summary>
    /// Contains extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Defines the epoch.
        /// </summary>
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Converts to a <see cref="DateTime"/> from epoch (ISO 8601) datetime.
        /// </summary>
        /// <param name="unixTime">The epoch seconds to convert.<see cref="long"/></param>
        public static DateTime FromEpoch(this long unixTime)
        {
            return _epoch.AddSeconds(unixTime);
        }
    }
}