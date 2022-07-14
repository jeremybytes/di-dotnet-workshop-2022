namespace DataProcessor.Library;

public class DataLoader
{
    private string dataPath;

    public DataLoader()
    {
        var dataFile = "data.txt";
        dataPath = AppDomain.CurrentDomain.BaseDirectory + dataFile;
    }

    public List<string> LoadData()
    {
        var data = new List<string>();

        if (File.Exists(dataPath))
        {
            using (var reader = new StreamReader(dataPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }
        }

        return data;
    }
}
