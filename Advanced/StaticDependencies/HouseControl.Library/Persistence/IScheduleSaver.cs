﻿using System.Collections.Generic;

namespace HouseControl.Library
{
    public interface IScheduleSaver
    {
        void SaveScheduleItems(string filename, IEnumerable<ScheduleItem> schedule);
    }
}
