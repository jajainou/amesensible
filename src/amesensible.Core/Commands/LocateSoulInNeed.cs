using amesensible.Core.Interfaces;
using amesensible.Core.Model;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace amesensible.Core.Commands
{
    public class LocateSoulInNeedCommand : IRequest<IReadOnlyCollection<SoulInNeed>>
    {
        public LocateSoulInNeedCommand(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public float Latitude { get; private set; }
        public float Longitude { get; private set; }
    }

    public class LocateSoulInNeedHandler : IRequestHandler<LocateSoulInNeedCommand, IEnumerable<SoulInNeed>>
    {
        private const int NearbyDistance = 10;
        private readonly ISoulInNeedRepository _soulInNeedRepository;

        public LocateSoulInNeedHandler(ISoulInNeedRepository soulInNeedRepository)
        {
            _soulInNeedRepository = soulInNeedRepository;
        }

        public async Task<IEnumerable<SoulInNeed>> Handle(LocateSoulInNeedCommand request, CancellationToken cancellationToken)
        {
            var ames = await _soulInNeedRepository.GetSoulInNeedNearbyPosition(request.Latitude, request.Longitude, NearbyDistance);
            if (ames.Any()) return ames;

            var gps = new GpsCoordinate(request.Latitude, request.Longitude);
            var ameBesoin = new SoulInNeed(gps, true);
            _soulInNeedRepository.Add(ameBesoin);

            return new SoulInNeed[] { ameBesoin };
        }
    }
}
