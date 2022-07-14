using PeopleViewer.Common;
using System.Text.Json;

namespace PersonDataReader.Service;

public class ServiceReader : IPersonReader
{
    HttpClient client = new HttpClient();
    JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

    public ServiceReader(ServiceReaderUri baseUri)
    {
        client.BaseAddress = new Uri(baseUri.ServiceUriString);
    }

    public async Task<IReadOnlyCollection<Person>> GetPeople()
    {
        HttpResponseMessage response = await client.GetAsync("people");
        if (response.IsSuccessStatusCode)
        {
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Person>>(stringResult, options) ?? new List<Person>();
        }
        return new List<Person>();
    }

    public async Task<Person?> GetPerson(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"people/{id}");
        if (response.IsSuccessStatusCode)
        {
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Person>(stringResult, options);
        }
        return new Person();
    }
}