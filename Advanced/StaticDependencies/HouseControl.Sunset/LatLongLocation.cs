namespace HouseControl.Sunset
{
    public class LatLongLocation
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public LatLongLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
