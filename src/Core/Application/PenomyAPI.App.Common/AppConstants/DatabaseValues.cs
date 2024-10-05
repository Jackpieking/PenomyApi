using System;

namespace PenomyAPI.App.Common.AppConstants;

public static class CommonValues
{
    public static class DateTimes
    {
        /// <summary>
        ///     This min value will be used when creating a new entity that
        ///     has TemporarilyRemovedAt property is not needed to used immediately.
        /// </summary>
        /// <remarks>
        ///     The below value is referenced from: <see href="https://penntoday.upenn.edu/news/worlds-first-general-purpose-computer-turns-75"/>
        /// </remarks>
        public static readonly DateTime MinUtc = new(1946, 2, 14, 0, 0, 0, DateTimeKind.Utc);
    }
}
