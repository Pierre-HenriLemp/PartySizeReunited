using System.Linq;
using PartySizeReunited.Services;
using TaleWorlds.CampaignSystem.GameComponents;
using Xunit;

namespace PartySizeReunited.Tests.Tests
{
    public class PartySizeModelDiscoveryServiceTests
    {
        [Fact]
        public void DiscoverPartySizeModels_FindsConcreteParameterlessDerivedTypes()
        {
            var service = new PartySizeModelDiscoveryService();

            var models = service.DiscoverPartySizeModels();

            Assert.Contains(models, m => m.GetType() == typeof(DiscoverableModel));
            Assert.DoesNotContain(models, m => m.GetType() == typeof(NoParameterlessCtorModel));
            Assert.DoesNotContain(models, m => m.GetType() == typeof(AbstractModel));
            Assert.DoesNotContain(models, m => m.GetType() == typeof(PartySizeReunited.PartySize));
        }

        [Fact]
        public void DiscoverPartySizeModels_DoesNotThrow()
        {
            var service = new PartySizeModelDiscoveryService();
            var ex = Record.Exception(() => service.DiscoverPartySizeModels().ToList());
            Assert.Null(ex);
        }

        public class DiscoverableModel : DefaultPartySizeLimitModel
        {
        }

        public class NoParameterlessCtorModel : DefaultPartySizeLimitModel
        {
            public NoParameterlessCtorModel(int ignored)
            {
            }
        }

        public abstract class AbstractModel : DefaultPartySizeLimitModel
        {
        }
    }
}
