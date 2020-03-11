using amesensible.Core.Model;
using amesensible.Data;
using amesensible.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace amesensible.Specs.Business.StepDefinitions
{
    [Binding]
    public class ASensitiveSoulHelpsASoulInNeedSteps
    {
        SensitiveSoul _sensitiveSoul;
        [Given(@"a Sensitive Soul position latitude (.*) and longitude (.*)")]
        public void GivenASensitiveSoulPositionLatitudeAndLongitude(float latitude, float longitude)
        {
            var gpsCoordinate = new GpsCoordinate(latitude, longitude);
            _sensitiveSoul = new SensitiveSoul(gpsCoordinate);
        }

        [When(@"he searches Souls In Need within (.*) meters")]
        public async Task WhenHeSearchesSoulsInNeed(int searchDistance)
        {
            var mdbContext = new Mock<AmeSensibleContext>();
            var soulInNeedRepository = new SoulInNeedRepository(mdbContext.Object);
            ScenarioStepContext.Current["GetSoulInNeedNearbyPosition"] =
                await _soulInNeedRepository.GetSoulInNeedNearbyPosition(_sensitiveSoul.GpsCoordinates, searchDistance);
        }

        [Then(@"the Souls In Need are listed")]
        public void ThenTheSoulsInNeedWithinMetersAreListed()
        {
            var result = (IEnumerable<SoulInNeed>)ScenarioStepContext.Current["GetSoulInNeedNearbyPosition"];
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(r => (r.GpsCoordinates - _sensitiveSoul.GpsCoordinates) <= 10));
        }
    }
}
