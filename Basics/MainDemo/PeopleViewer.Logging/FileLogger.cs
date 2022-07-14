namespace PeopleViewer.Logging
{
    public class FileLogger : ILogger
    {
        string filePath;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public void LogException(Exception ex)
        {
            using var sr = new StreamWriter(filePath, true);
            string message = "--------------------------------------";
            message += Environment.NewLine;
            message += $"START {DateTime.Now}{Environment.NewLine}";
            message += $"EXCEPTION{Environment.NewLine}";
            message += $"{ex}{Environment.NewLine}";
            message += $"END {DateTime.Now}{Environment.NewLine}";
            message += "--------------------------------------";

            sr.WriteLine(message);
        }

        public void LogMessage(string message)
        {
            using var sr = new StreamWriter(filePath, true);
            sr.WriteLine($"MESSAGE {DateTime.Now}: {message}");
        }
    }
}