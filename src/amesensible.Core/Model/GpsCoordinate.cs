using System.Collections.Generic;

namespace amesensible.Core.Model
{
    public class GpsCoordinate : ValueObject
    {
        public GpsCoordinate(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public float Latitude { get; private set; }
        public float Longitude { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Latitude;
            yield return Longitude;
        }
    }
}
