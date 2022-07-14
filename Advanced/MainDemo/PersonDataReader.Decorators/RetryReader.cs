﻿using PeopleViewer.Common;

namespace PersonDataReader.Decorators;

public class RetryReader : IPersonReader
{
    private IPersonReader _wrappedReader;
    private TimeSpan _retryDelay;
    private int _retryCount = 0;

    public RetryReader(IPersonReader wrappedReader,
        TimeSpan retryDelay)
    {
        _wrappedReader = wrappedReader;
        _retryDelay = retryDelay;
    }

    public async Task<IReadOnlyCollection<Person>> GetPeople()
    {
        _retryCount++;
        try
        {
            var people = await _wrappedReader.GetPeople();
            _retryCount = 0;
            return people;
        }
        catch (Exception)
        {
            if (_retryCount >= 3)
                throw;
            await Task.Delay(_retryDelay);
            return await this.GetPeople();
        }
    }

    public async Task<Person?> GetPerson(int id)
    {
        _retryCount++;
        try
        {
            var person = await _wrappedReader.GetPerson(id);
            _retryCount = 0;
            return person;
        }
        catch (Exception)
        {
            if (_retryCount >= 3)
                throw;
            await Task.Delay(_retryDelay);
            return await this.GetPerson(id);
        }
    }

    public string GetTypeName()
    {
        return $"{this.GetType().Name} ({_wrappedReader.GetTypeName()})";
    }
}
