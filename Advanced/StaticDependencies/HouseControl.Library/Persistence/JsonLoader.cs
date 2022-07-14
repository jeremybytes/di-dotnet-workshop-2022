using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace HouseControl.Library
{
    public class JsonLoader : IScheduleLoader
    {
        public IEnumerable<ScheduleItem> LoadScheduleItems(string filename)
        {
            var output = new List<ScheduleItem>();
            filename = filename + ".json";

            var schedule = new List<ScheduleItem>();
            if (File.Exists(filename))
            {
                using (var reader = new StreamReader(filename))
                {
                    output = JsonConvert.DeserializeObject<List<ScheduleItem>>(
                        reader.ReadToEnd());
                }
            }

            return output;
        }
    }
}
