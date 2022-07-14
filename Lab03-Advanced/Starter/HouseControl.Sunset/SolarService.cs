namespace HouseControl.Sunset;

public interface ISolarService
{
    string GetServiceData(DateTime date);
}

public class SolarService : ISolarService
{
    private static readonly HttpClient client = 
        new() { BaseAddress = new Uri("http://localhost:8973") };

    public string GetServiceData(DateTime date)
    {
        HttpResponseMessage response =
            client.GetAsync($"SolarCalculator/33.7676/-84.5606/{date:yyyy-MM-dd}").Result;

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Unable to complete request: status code {response.StatusCode}");

        var stringResult =
            response.Content.ReadAsStringAsync().Result;

        return stringResult;
    }
}
