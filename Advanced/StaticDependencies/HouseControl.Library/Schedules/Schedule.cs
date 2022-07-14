using HouseControl.Sunset;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HouseControl.Library
{
    public class Schedule : List<ScheduleItem>
    {
        private string filename;

        private IScheduleLoader loader;
        public IScheduleLoader Loader
        {
            get
            {
                if (loader == null)
                    loader = new JsonLoader();
                return loader;
            }
            set
            {
                if (loader != value)
                    loader = value;
            }
        }

        private IScheduleSaver saver;
        public IScheduleSaver Saver
        {
            get
            {
                if (saver == null)
                    saver = new JsonSaver();
                return saver;
            }
            set
            {
                if (saver != value)
                    saver = value;
            }
        }

        private readonly ScheduleHelper scheduleHelper;

        public Schedule(ScheduleFileName filename, ISunsetProvider sunsetProvider)
        {
            this.scheduleHelper = new ScheduleHelper(sunsetProvider);
            this.filename = filename.FileName;
            LoadSchedule();
        }

        public void LoadSchedule()
        {
            this.Clear();
            this.AddRange(Loader.LoadScheduleItems(filename));
            RollSchedule();
        }

        public void SaveSchedule()
        {
            Saver.SaveScheduleItems(filename, this);
        }

        public List<ScheduleItem> GetCurrentScheduleItems()
        {
            return this.Where(si => si.IsEnabled &&
                ScheduleHelper.DurationFromNow(si.Info.EventTime) < TimeSpan.FromSeconds(30))
                .ToList();
        }

        public void RollSchedule()
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                var currentItem = this[i];
                while (ScheduleHelper.IsInPast(currentItem.Info.EventTime))
                {
                    if (currentItem.Info.Type == ScheduleType.Once)
                    {
                        this.RemoveAt(i);
                        break;
                    }

                    switch (currentItem.Info.Type)
                    {
                        case ScheduleType.Daily:
                            currentItem.Info.EventTime =
                                scheduleHelper.RollForwardToNextDay(currentItem.Info);
                            break;
                        case ScheduleType.Weekday:
                            currentItem.Info.EventTime =
                                scheduleHelper.RollForwardToNextWeekdayDay(currentItem.Info);
                            break;
                        case ScheduleType.Weekend:
                            currentItem.Info.EventTime =
                                scheduleHelper.RollForwardToNextWeekendDay(currentItem.Info);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

    }
}
