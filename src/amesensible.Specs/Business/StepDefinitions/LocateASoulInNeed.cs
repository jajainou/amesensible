using amesensible.Core.Commands;
using amesensible.Core.Interfaces;
using amesensible.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace amesensible.Specs.Business.StepDefinitions
{
    [Binding]
    public class LocateASoulInNeed
    {
        private User _user;
        private LocateSoulInNeedCommand _locateAbCommand;

        [Given(@"An application user")]
        public void GivenUnUtilisateurDeLApplication()
        {
            _user = new User();
        }

        [When(@"He provides a latitude (.*) and a longitude (.*)")]
        public void WhenIlFournitUneLatitude_EtUneLongitutde_(float latitude, float longitude)
        {
            _locateAbCommand = new LocateSoulInNeedCommand(latitude, longitude);
        }

        [Then(@"A Soul In Need is created")]
        public async Task ThenUbABEstCree()
        {
            var mRepos = new Mock<ISoulInNeedRepository>();
            //search ab nearby
            mRepos.Setup(r => r.GetSoulInNeedNearbyPosition(It.IsAny<GpsCoordinate>(), It.IsAny<int>()))
            .Returns(Task.FromResult(new SoulInNeed[] { }.AsEnumerable()));
            //
            mRepos.Setup(r => r.Add(It.IsAny<SoulInNeed>()));

            var handler = new LocateSoulInNeedHandler(mRepos.Object);
            var cToken = new CancellationToken();

            var result = await handler.Handle(_locateAbCommand, cToken);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.First().IsNew);
        }

        [Then(@"A Soul In Need is returned if he is within (.*) meters")]
        public async Task ThenUnABEstRetourne(int distance)
        {
            var mRepos = new Mock<ISoulInNeedRepository>();
            //search ab nearby
            mRepos.Setup(r => r.GetSoulInNeedNearbyPosition(It.IsAny<GpsCoordinate>(), distance))
            .Returns(Task.FromResult(new SoulInNeed[] { new SoulInNeed(new GpsCoordinate(_locateAbCommand.Latitude, _locateAbCommand.Longitude), false) }.AsEnumerable()));

            var handler = new LocateSoulInNeedHandler(mRepos.Object);
            var cToken = new CancellationToken();

            var result = await handler.Handle(_locateAbCommand, cToken);
            Assert.IsTrue(result.Any());
            Assert.IsFalse(result.First().IsNew);
            mRepos.Verify(r => r.Add(It.IsAny<SoulInNeed>()), Times.Never);
        }
    }
}
