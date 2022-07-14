using PeopleViewer.Common;
using PeopleViewer.Logging;

namespace PersonDataReader.Decorators;

public class ExceptionLoggingReader : IPersonReader
{
    IPersonReader _wrappedReader;
    ILogger _logger;

    public ExceptionLoggingReader(IPersonReader wrappedReader,
        ILogger logger)
    {
        _wrappedReader = wrappedReader;
        _logger = logger;
    }

    public Task<IReadOnlyCollection<Person>> GetPeople()
    {
        try
        {
            return _wrappedReader.GetPeople();
        }
        catch (Exception ex)
        {
            _logger?.LogException(ex);
            throw;
        }
    }

    public Task<Person?> GetPerson(int id)
    {
        try
        {
            return _wrappedReader.GetPerson(id);
        }
        catch (Exception ex)
        {
            _logger?.LogException(ex);
            throw;
        }
    }

    public string GetTypeName()
    {
        return $"{this.GetType().Name} ({_wrappedReader.GetTypeName()})";
    }
}
