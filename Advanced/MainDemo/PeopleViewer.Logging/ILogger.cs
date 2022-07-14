namespace PeopleViewer.Logging;

public interface ILogger
{
    void LogException(Exception ex);
    void LogMessage(string message);
}
