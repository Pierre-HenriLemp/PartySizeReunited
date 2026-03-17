using PartySizeReunited.Models;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using Xunit;

namespace PartySizeReunited.Tests.Tests
{
    public class ScopeExtensionNullSafetyTests
    {
        [Fact]
        public void PartyBase_Methods_DoNotThrow_WhenPartyIsNull()
        {
            var ex1 = Record.Exception(() => ScopeExtension.IsOnlyPlayer((PartyBase)null!));
            var ex2 = Record.Exception(() => ScopeExtension.IsOnlyEnnemies((PartyBase)null!));
            var ex3 = Record.Exception(() => ScopeExtension.IsOnlyPlayerClan((PartyBase)null!));
            var ex4 = Record.Exception(() => ScopeExtension.IsOnlyPlayerKingdom((PartyBase)null!));
            var ex5 = Record.Exception(() => ScopeExtension.IsEveryoneExceptPlayer((PartyBase)null!));

            Assert.Null(ex1);
            Assert.Null(ex2);
            Assert.Null(ex3);
            Assert.Null(ex4);
            Assert.Null(ex5);
        }

        [Fact]
        public void MobileParty_Methods_DoNotThrow_WhenPartyIsNull()
        {
            var ex1 = Record.Exception(() => ScopeExtension.IsOnlyPlayer((MobileParty)null!));
            var ex2 = Record.Exception(() => ScopeExtension.IsOnlyEnnemies((MobileParty)null!));
            var ex3 = Record.Exception(() => ScopeExtension.IsOnlyPlayerClan((MobileParty)null!));
            var ex4 = Record.Exception(() => ScopeExtension.IsOnlyPlayerKingdom((MobileParty)null!));
            var ex5 = Record.Exception(() => ScopeExtension.IsEveryoneExceptPlayer((MobileParty)null!));

            Assert.Null(ex1);
            Assert.Null(ex2);
            Assert.Null(ex3);
            Assert.Null(ex4);
            Assert.Null(ex5);
        }

        [Fact]
        public void Clan_And_Kingdom_Methods_DoNotThrow_WhenMainHeroIsNull()
        {
            Hero.MainHero = null;

            var clan = new Clan();
            var kingdom = new Kingdom { Name = "Aserai" };

            var ex1 = Record.Exception(() => ScopeExtension.IsOnlyPlayerClan(clan));
            var ex2 = Record.Exception(() => ScopeExtension.IsOnlyPlayerKingdom(kingdom));
            var ex3 = Record.Exception(() => ScopeExtension.IsOnlyEnnemies(kingdom));

            Assert.Null(ex1);
            Assert.Null(ex2);
            Assert.Null(ex3);
        }
    }
}
