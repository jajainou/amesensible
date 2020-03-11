using System;
using System.Collections.Generic;

namespace amesensible.Core.Model
{
    public class GpsCoordinate : ValueObject
    {
        private const double EarthRadius = 6376500.0;

        public GpsCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Latitude;
            yield return Longitude;
        }

        public static double operator -(GpsCoordinate gps1, GpsCoordinate gps2)
        {
            var d1 = gps1.Latitude * (Math.PI / 180.0);
            var num1 = gps1.Longitude * (Math.PI / 180.0);
            var d2 = gps2.Latitude * (Math.PI / 180.0);
            var num2 = gps2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return EarthRadius * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public (double minLat, double maxLat, double minLon, double maxLon) GetBoundingBox(int distance)
        {
            if (distance < 0d)
                throw new Exception("Distance cannot be less than 0");

            // angular distance in radians on a great circle
            double radDist = distance / EarthRadius;

            double radLat = Latitude * (Math.PI / 180.0);
            double radLon = Longitude * (Math.PI / 180.0);

            double minLat = radLat - radDist;
            double maxLat = radLat + radDist;

            double deltaLon = Math.Asin(Math.Sin(radDist) / Math.Cos(radLat));
            double minLon = radLon - deltaLon;
            double maxLon = radLon + deltaLon;
            return (minLat, maxLat, minLon, maxLon);
        }
    }
}
