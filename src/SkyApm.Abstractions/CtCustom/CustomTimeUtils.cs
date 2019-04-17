using System;

namespace SkyApm.Abstractions.Common
{
    public static class CustomTimeUtils
    {
        /// <summary>
        /// Returns the number of milliseconds that have elapsed since 1970-01-01T00:00:00.000Z.
        /// </summary>
        /// <param name="time"></param>
        /// <returns>The number of milliseconds that have elapsed since 1970-01-01T00:00:00.000Z.</returns>
        public static long ConvertToUnixTimeMilliseconds(this DateTimeOffset time)
        {
#if NET_FX
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(time.ToUniversalTime() - epoch).TotalMilliseconds;
#else
        return time.ToUnixTimeMilliseconds();
#endif
        }
    }
}
