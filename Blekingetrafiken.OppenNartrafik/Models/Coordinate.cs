using System.Globalization;

namespace Blekingetrafiken.OppenNartrafik.Models
{
    public class Coordinate
    {
        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinate(string latitude, string longitude)
        {
            Latitude = double.Parse(latitude, CultureInfo.InvariantCulture);
            Longitude = double.Parse(longitude, CultureInfo.InvariantCulture);
        }

        public double Latitude { get; }
        public double Longitude { get; }
    }
}
