using System.Text.Json;

namespace HouseControl.Sunset;

public class SolarServiceSunsetProvider : ISunsetProvider
{
#pragma warning disable IDE1006 // Naming Styles
    private record Results(string sunrise, string sunset, string solar_noon, string day_length);
    private record SolarData(Results results, string status);
#pragma warning restore IDE1006 // Naming Styles

    private ISolarService? service;
    public ISolarService Service
    {
        get => service ??= new SolarService();
        set => service = value;
    }

    public DateTimeOffset GetSunrise(DateTime date)
    {
        string serviceData = Service.GetServiceData(date);
        string sunriseTimeString = ParseSunriseTime(serviceData);
        DateTime sunriseTime = ToLocalTime(sunriseTimeString, date);
        return new DateTimeOffset(sunriseTime);
    }

    public DateTimeOffset GetSunset(DateTime date)
    {
        string serviceData = Service.GetServiceData(date);
        string sunsetTimeString = ParseSunsetTime(serviceData);
        DateTime sunsetTime = ToLocalTime(sunsetTimeString, date);
        return new DateTimeOffset(sunsetTime);
    }

    public static DateTime ToLocalTime(string inputTime, DateTime date)
    {
        DateTime time = DateTime.Parse(inputTime);
        DateTime result = date.Date + time.TimeOfDay;
        return result;
    }

    public static string ParseSunsetTime(string jsonData)
    {
        SolarData? data = JsonSerializer.Deserialize<SolarData>(jsonData);
        string sunsetTimeString = data!.results.sunset;
        return sunsetTimeString;
    }

    public static string ParseSunriseTime(string jsonData)
    {
        SolarData? data = JsonSerializer.Deserialize<SolarData>(jsonData);
        return data!.results.sunrise;
    }

}
