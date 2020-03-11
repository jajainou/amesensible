using amesensible.Core.Interfaces;
using amesensible.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace amesensible.Data.Repositories
{
    public class SoulInNeedRepository : EfRepository<SoulInNeed>, ISoulInNeedRepository
    {
        public SoulInNeedRepository(AmeSensibleContext ameSensibleContext) : base(ameSensibleContext)
        {
        }

        public Task<IEnumerable<SoulInNeed>> GetSoulInNeedNearbyPosition(GpsCoordinate gps, int distance)
        {
            var bbox = gps.GetBoundingBox(distance);
            var results = this.GetWhere(sn =>
            sn.GpsCoordinates.Latitude >= bbox.minLat && sn.GpsCoordinates.Latitude <= bbox.maxLat
            && sn.GpsCoordinates.Longitude >= bbox.minLon && sn.GpsCoordinates.Longitude <= bbox.maxLon)
                .AsEnumerable();
            return Task.FromResult(results);
        }
    }
}
