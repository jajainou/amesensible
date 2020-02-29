namespace amesensible.Core.Model
{
    public class SensitiveSoul : Entity
    {
        public SensitiveSoul(GpsCoordinate gpsCoordinates)
        {
            GpsCoordinates = gpsCoordinates;
        }

        public GpsCoordinate GpsCoordinates { get; private set; }
    }
}
