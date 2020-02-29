using amesensible.Core.Model;
using amesensible.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace amesensible.Specs.Business.StepDefinitions
{
    [Binding]
    public class ASensitiveSoulHelpsASoulInNeedSteps
    {
        SensitiveSoul _sensitiveSoul;
        SoulInNeedRepository _soulInNeedRepository;
        [Given(@"a Sensitive Soul position latitude (.*) and longitude (.*)")]
        public void GivenASensitiveSoulPositionLatitudeAndLongitude(float latitude, float longitude)
        {
            var gpsCoordinate = new GpsCoordinate(latitude, longitude);
            _sensitiveSoul = new SensitiveSoul(gpsCoordinate);
        }

        [When(@"he searches Souls In Need within (.*) meters")]
        public async Task WhenHeSearchesSoulsInNeed(int searchDistance)
        {
            var latitude = _sensitiveSoul.GpsCoordinates.Latitude;
            var longitude = _sensitiveSoul.GpsCoordinates.Longitude;
            ScenarioStepContext.Current["GetSoulInNeedNearbyPosition"] =
                await _soulInNeedRepository.GetSoulInNeedNearbyPosition(latitude, longitude, searchDistance);
        }

        [Then(@"the Souls In Need are listed")]
        public void ThenTheSoulsInNeedWithinMetersAreListed()
        {
            var result = (IEnumerable<SoulInNeed>)ScenarioStepContext.Current["GetSoulInNeedNearbyPosition"];
        }
    }
}
