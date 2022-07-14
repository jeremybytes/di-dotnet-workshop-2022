using PeopleViewer.Common;

namespace PersonDataReader.SQL;

public class SQLReaderProxy : IPersonReader
{
    string sqlFileName;

    public SQLReaderProxy(string sqlFileName)
    {
        this.sqlFileName = sqlFileName;
    }

    public async Task<IReadOnlyCollection<Person>> GetPeople()
    {
        using var reader = new SQLReader(sqlFileName);
        return await reader.GetPeople();
    }

    public async Task<Person?> GetPerson(int id)
    {
        using var reader = new SQLReader(sqlFileName);
        return await reader.GetPerson(id);
    }
}
