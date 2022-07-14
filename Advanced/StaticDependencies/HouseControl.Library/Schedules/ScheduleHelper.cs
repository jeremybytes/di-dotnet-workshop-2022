using HouseControl.Sunset;
using System;

namespace HouseControl.Library
{
    public class ScheduleHelper
    {
        private static ITimeProvider timeProvider;
        public static ITimeProvider TimeProvider
        {
            get
            {
                if (timeProvider == null)
                    timeProvider = new CurrentTimeProvider();
                return timeProvider;
            }
            set { timeProvider = value; }
        }

        private readonly ISunsetProvider SunsetProvider;

        public ScheduleHelper(ISunsetProvider sunsetProvider)
        {
            this.SunsetProvider = sunsetProvider;
        }

        public static DateTimeOffset Now()
        {
            return TimeProvider.Now();
        }

        public static DateTime Today()
        {
            return TimeProvider.Now().Date;
        }

        public static DateTime Yesterday()
        {
            return TimeProvider.Now().Date.AddDays(-1);
        }

        public static TimeSpan DurationFromNow(DateTimeOffset checkTime)
        {
            return (checkTime - TimeProvider.Now()).Duration();
        }

        public static bool IsInPast(DateTimeOffset checkTime)
        {
            return checkTime < TimeProvider.Now();
        }

        public DateTimeOffset RollForwardToNextDay(ScheduleInfo info)
        {
            if (IsInPast(info.EventTime))
            {
                var nextDay = Today().AddDays(1);
                switch (info.TimeType)
                {
                    case ScheduleTimeType.Standard:
                        return nextDay + info.EventTime.TimeOfDay + info.RelativeOffset;
                    case ScheduleTimeType.Sunset:
                        return SunsetProvider.GetSunset(nextDay) + info.RelativeOffset;
                    case ScheduleTimeType.Sunrise:
                        return SunsetProvider.GetSunrise(nextDay) + info.RelativeOffset;
                }
            }
            return info.EventTime;
        }

        public DateTimeOffset RollForwardToNextWeekdayDay(ScheduleInfo info)
        {
            if (IsInPast(info.EventTime))
            {
                var nextDay = Today() + TimeSpan.FromDays(1);
                while (nextDay.DayOfWeek == DayOfWeek.Saturday
                    || nextDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    nextDay = nextDay.AddDays(1);
                }
                switch (info.TimeType)
                {
                    case ScheduleTimeType.Standard:
                        return nextDay + info.EventTime.TimeOfDay + info.RelativeOffset;
                    case ScheduleTimeType.Sunset:
                        return SunsetProvider.GetSunset(nextDay) + info.RelativeOffset;
                    case ScheduleTimeType.Sunrise:
                        return SunsetProvider.GetSunrise(nextDay) + info.RelativeOffset;
                }
            }
            return info.EventTime;
        }

        public DateTimeOffset RollForwardToNextWeekendDay(ScheduleInfo info)
        {
            if (IsInPast(info.EventTime))
            {
                var nextDay = Today().AddDays(1);
                while (nextDay.DayOfWeek != DayOfWeek.Saturday
                    && nextDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    nextDay = nextDay = nextDay.AddDays(1);
                }
                switch (info.TimeType)
                {
                    case ScheduleTimeType.Standard:
                        return nextDay + info.EventTime.TimeOfDay + info.RelativeOffset;
                    case ScheduleTimeType.Sunset:
                        return SunsetProvider.GetSunset(nextDay) + info.RelativeOffset;
                    case ScheduleTimeType.Sunrise:
                        return SunsetProvider.GetSunrise(nextDay) + info.RelativeOffset;
                }
            }
            return info.EventTime;
        }
    }
}
