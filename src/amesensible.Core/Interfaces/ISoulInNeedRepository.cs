using amesensible.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace amesensible.Core.Interfaces
{
    public interface ISoulInNeedRepository : IRepository<SoulInNeed>
    {
        Task<IEnumerable<SoulInNeed>> GetSoulInNeedNearbyPosition(GpsCoordinate gps, int distance);
    }
}
