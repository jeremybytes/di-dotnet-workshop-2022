using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace HouseControl.Library
{
    public class JsonSaver : IScheduleSaver
    {
        public void SaveScheduleItems(string filename, IEnumerable<ScheduleItem> schedule)
        {
            filename = filename + ".json";

            var output = JsonConvert.SerializeObject(schedule, Formatting.Indented);

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine(output);
            }
        }
    }
}
