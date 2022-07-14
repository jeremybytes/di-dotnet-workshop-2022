using System.Collections.Generic;

namespace HouseControl.Library
{
    public interface IScheduleLoader
    {
        IEnumerable<ScheduleItem> LoadScheduleItems(string filename);
    }
}
