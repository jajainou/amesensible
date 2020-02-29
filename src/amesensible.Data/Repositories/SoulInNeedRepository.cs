using amesensible.Core.Interfaces;
using amesensible.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace amesensible.Data.Repositories
{
    public class SoulInNeedRepository : EfRepository<SoulInNeed>, ISoulInNeedRepository
    {
        public SoulInNeedRepository(AmeSensibleContext ameSensibleContext) : base(ameSensibleContext)
        {
        }

        public Task<IEnumerable<SoulInNeed>> GetSoulInNeedNearbyPosition(float latitude, float longitude, int distance)
        {

        }
    }
}
