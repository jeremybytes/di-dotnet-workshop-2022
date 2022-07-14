using DataProcessor.Library;

namespace DataProcessor;

class Program
{
    static void Main(string[] args)
    {
        int recordsProcessed = ProcessData();

        Console.WriteLine($"Successfully processed {recordsProcessed} records");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private static int ProcessData()
    {
        var loader = new DataLoader();
        List<string> data = loader.LoadData();

        ILogger logger = new FileLogger();
        var parser = new DataParser(logger);
        int recordsProcessed = parser.ParseData(data);
        return recordsProcessed;
    }
}
