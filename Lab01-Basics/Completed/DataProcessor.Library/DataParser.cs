namespace DataProcessor.Library;

public class DataParser
{
    //FileLogger logger = new();
    private ILogger logger;

    public DataParser(ILogger logger)
    {
        this.logger = logger;
    }

    public int ParseData(List<string> data)
    {
        var recordsProcessed = 0;
        foreach (var record in data)
        {
            var fields = record.Split(',');
            if (fields.Length != 4)
            {
                logger.Log("Wrong number of fields in record", record);
                continue;
            }

            DateTime startDate;
            if (!DateTime.TryParse(fields[2], out startDate))
            {
                logger.Log("Cannot parse Start Date field", record);
                continue;
            }

            int rating;
            if (!Int32.TryParse(fields[3], out rating))
            {
                logger.Log("Cannot parse Rating field", record);
                continue;
            }

            // Successfully parsed record
            recordsProcessed++;
        }
        return recordsProcessed;
    }
}
