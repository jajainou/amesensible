namespace amesensible.Core.Model
{
    public class SoulInNeed : Entity
    {
        public GpsCoordinate GpsCoordinates { get; private set; }
        public bool IsNew { get; private set; }

        public SoulInNeed()
        {
        }
        public SoulInNeed(GpsCoordinate gps, bool isNew)
        {
            GpsCoordinates = gps;
            IsNew = isNew;
        }
    }
}
