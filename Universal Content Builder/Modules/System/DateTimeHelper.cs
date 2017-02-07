using System;

namespace Modules.System
{
    public static class DateTimeHelper
    {
        public static long ToUnixTime(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds.Truncate().ToString().ToLong();
        }

        public static DateTime ToDateTime(this long unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime).ToLocalTime();
        }
    }
}