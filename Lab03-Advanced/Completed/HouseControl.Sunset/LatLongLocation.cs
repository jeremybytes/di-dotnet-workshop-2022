namespace HouseControl.Sunset;

public class LatLongLocation
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    public LatLongLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
