using System;

namespace HouseControl.Library
{
    public class ScheduleItem
    {
        public string ScheduleSet { get; set; }
        public int Device { get; set; }
        public ScheduleInfo Info { get; set; }
        public DeviceCommands Command { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class ScheduleInfo
    {
        public DateTimeOffset EventTime { get; set; }
        public ScheduleTimeType TimeType { get; set; }
        public TimeSpan RelativeOffset { get; set; }
        public ScheduleType Type { get; set; }
    }
}
