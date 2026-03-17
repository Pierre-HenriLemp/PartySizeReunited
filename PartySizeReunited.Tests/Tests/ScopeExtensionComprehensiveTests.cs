using PartySizeReunited.Models;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using Xunit;

namespace PartySizeReunited.Tests.Tests
{
    public class ScopeExtensionComprehensiveTests
    {
        [Fact]
        public void PartyBase_IsOnlyPlayer_ReturnsExpectedValues()
        {
            var playerClan = new Clan { Kingdom = new Kingdom { Name = "Vlandia" } };
            var playerParty = new PartyBase { Owner = new Hero { IsHumanPlayerCharacter = true, Clan = playerClan } };
            var aiParty = new PartyBase { Owner = new Hero { IsHumanPlayerCharacter = false, Clan = playerClan } };

            Assert.True(ScopeExtension.IsOnlyPlayer(playerParty));
            Assert.False(ScopeExtension.IsOnlyPlayer(aiParty));
            Assert.False(ScopeExtension.IsOnlyPlayer((PartyBase)null!));
        }

        [Fact]
        public void PartyBase_IsEveryoneExceptPlayer_ReturnsExpectedValues()
        {
            var playerParty = new PartyBase { Owner = new Hero { IsHumanPlayerCharacter = true } };
            var aiParty = new PartyBase { Owner = new Hero { IsHumanPlayerCharacter = false } };

            Assert.False(ScopeExtension.IsEveryoneExceptPlayer(playerParty));
            Assert.True(ScopeExtension.IsEveryoneExceptPlayer(aiParty));
            Assert.True(ScopeExtension.IsEveryoneExceptPlayer((PartyBase)null!));
        }

        [Fact]
        public void PartyBase_IsOnlyPlayerClan_ReturnsExpectedValues()
        {
            var playerClan = new Clan { Kingdom = new Kingdom { Name = "Vlandia" } };
            Hero.MainHero = new Hero { Clan = playerClan };

            var sameClanAiParty = new PartyBase { Owner = new Hero { IsHumanPlayerCharacter = false, Clan = playerClan } };
            var differentClanAiParty = new PartyBase
            {
                Owner = new Hero { IsHumanPlayerCharacter = false, Clan = new Clan { Kingdom = playerClan.Kingdom } }
            };
            var playerParty = new PartyBase { Owner = new Hero { IsHumanPlayerCharacter = true, Clan = playerClan } };

            Assert.True(ScopeExtension.IsOnlyPlayerClan(sameClanAiParty));
            Assert.False(ScopeExtension.IsOnlyPlayerClan(differentClanAiParty));
            Assert.False(ScopeExtension.IsOnlyPlayerClan(playerParty));
            Assert.False(ScopeExtension.IsOnlyPlayerClan((PartyBase)null!));
        }

        [Fact]
        public void PartyBase_IsOnlyPlayerKingdom_ReturnsExpectedValues()
        {
            var playerKingdom = new Kingdom { Name = "Vlandia" };
            var playerClan = new Clan { Kingdom = playerKingdom };
            Hero.MainHero = new Hero { Clan = playerClan };

            var sameKingdomAiParty = new PartyBase
            {
                Owner = new Hero { IsHumanPlayerCharacter = false, Clan = new Clan { Kingdom = new Kingdom { Name = "Vlandia" } } }
            };
            var differentKingdomAiParty = new PartyBase
            {
                Owner = new Hero { IsHumanPlayerCharacter = false, Clan = new Clan { Kingdom = new Kingdom { Name = "Aserai" } } }
            };

            Assert.True(ScopeExtension.IsOnlyPlayerKingdom(sameKingdomAiParty));
            Assert.False(ScopeExtension.IsOnlyPlayerKingdom(differentKingdomAiParty));
            Assert.False(ScopeExtension.IsOnlyPlayerKingdom((PartyBase)null!));
        }

        [Fact]
        public void PartyBase_IsOnlyEnnemies_ReturnsExpectedValues()
        {
            var playerKingdom = new Kingdom { Name = "Vlandia" };
            var playerClan = new Clan { Kingdom = playerKingdom };
            Hero.MainHero = new Hero { Clan = playerClan };

            var sameClanAiParty = new PartyBase { Owner = new Hero { IsHumanPlayerCharacter = false, Clan = playerClan } };
            var sameKingdomDifferentClanAiParty = new PartyBase
            {
                Owner = new Hero { IsHumanPlayerCharacter = false, Clan = new Clan { Kingdom = new Kingdom { Name = "Vlandia" } } }
            };
            var differentKingdomAiParty = new PartyBase
            {
                Owner = new Hero { IsHumanPlayerCharacter = false, Clan = new Clan { Kingdom = new Kingdom { Name = "Aserai" } } }
            };
            var playerParty = new PartyBase { Owner = new Hero { IsHumanPlayerCharacter = true, Clan = playerClan } };

            Assert.True(ScopeExtension.IsOnlyEnnemies(sameClanAiParty));
            Assert.True(ScopeExtension.IsOnlyEnnemies(sameKingdomDifferentClanAiParty));
            Assert.True(ScopeExtension.IsOnlyEnnemies(differentKingdomAiParty));
            Assert.False(ScopeExtension.IsOnlyEnnemies(playerParty));
            Assert.True(ScopeExtension.IsOnlyEnnemies((PartyBase)null!));
        }

        [Fact]
        public void Clan_And_Kingdom_Overloads_ReturnExpectedValues()
        {
            var playerKingdom = new Kingdom { Name = "Vlandia" };
            var playerClan = new Clan { Kingdom = playerKingdom };
            Hero.MainHero = new Hero { Clan = playerClan };

            var sameClan = playerClan;
            var otherClan = new Clan { Kingdom = playerKingdom };
            var sameKingdom = playerKingdom;
            var sameNameDifferentInstanceKingdom = new Kingdom { Name = "Vlandia" };
            var otherKingdom = new Kingdom { Name = "Aserai" };

            Assert.True(ScopeExtension.IsOnlyPlayerClan(sameClan));
            Assert.False(ScopeExtension.IsOnlyPlayerClan(otherClan));
            Assert.False(ScopeExtension.IsOnlyPlayerClan((Clan)null!));

            Assert.True(ScopeExtension.IsOnlyPlayerKingdom(sameKingdom));
            Assert.False(ScopeExtension.IsOnlyPlayerKingdom(sameNameDifferentInstanceKingdom));
            Assert.False(ScopeExtension.IsOnlyPlayerKingdom((Kingdom)null!));

            Assert.True(ScopeExtension.IsOnlyEnnemies(otherKingdom));
            Assert.False(ScopeExtension.IsOnlyEnnemies(sameNameDifferentInstanceKingdom));
            Assert.False(ScopeExtension.IsOnlyEnnemies((Kingdom)null!));
        }

        [Fact]
        public void MobileParty_IsOnlyPlayer_And_EveryoneExceptPlayer_ReturnExpectedValues()
        {
            var mainParty = new MobileParty { IsMainParty = true };
            var aiParty = new MobileParty { IsMainParty = false };

            Assert.True(ScopeExtension.IsOnlyPlayer(mainParty));
            Assert.False(ScopeExtension.IsOnlyPlayer(aiParty));
            Assert.False(ScopeExtension.IsOnlyPlayer((MobileParty)null!));

            Assert.False(ScopeExtension.IsEveryoneExceptPlayer(mainParty));
            Assert.True(ScopeExtension.IsEveryoneExceptPlayer(aiParty));
            Assert.True(ScopeExtension.IsEveryoneExceptPlayer((MobileParty)null!));
        }

        [Fact]
        public void MobileParty_IsOnlyPlayerClan_And_IsOnlyPlayerKingdom_ReturnExpectedValues()
        {
            var playerKingdom = new Kingdom { Name = "Vlandia" };
            var playerClan = new Clan { Kingdom = playerKingdom };
            Hero.MainHero = new Hero { Clan = playerClan };

            var sameClanParty = new MobileParty { IsMainParty = false, ActualClan = playerClan };
            var sameKingdomDifferentClanParty = new MobileParty
            {
                IsMainParty = false,
                ActualClan = new Clan { Kingdom = new Kingdom { Name = "Vlandia" } }
            };
            var differentKingdomParty = new MobileParty
            {
                IsMainParty = false,
                ActualClan = new Clan { Kingdom = new Kingdom { Name = "Aserai" } }
            };

            Assert.True(ScopeExtension.IsOnlyPlayerClan(sameClanParty));
            Assert.False(ScopeExtension.IsOnlyPlayerClan(sameKingdomDifferentClanParty));
            Assert.False(ScopeExtension.IsOnlyPlayerClan((MobileParty)null!));

            Assert.True(ScopeExtension.IsOnlyPlayerKingdom(sameClanParty));
            Assert.True(ScopeExtension.IsOnlyPlayerKingdom(sameKingdomDifferentClanParty));
            Assert.False(ScopeExtension.IsOnlyPlayerKingdom(differentKingdomParty));
            Assert.False(ScopeExtension.IsOnlyPlayerKingdom((MobileParty)null!));
        }

        [Fact]
        public void MobileParty_IsOnlyEnnemies_ReturnsExpectedValues()
        {
            var playerKingdom = new Kingdom { Name = "Vlandia" };
            var playerClan = new Clan { Kingdom = playerKingdom };
            Hero.MainHero = new Hero { Clan = playerClan };

            var mainParty = new MobileParty { IsMainParty = true, ActualClan = playerClan };
            var nullKingdomParty = new MobileParty { IsMainParty = false, ActualClan = new Clan { Kingdom = null } };
            var sameKingdomParty = new MobileParty
            {
                IsMainParty = false,
                ActualClan = new Clan { Kingdom = new Kingdom { Name = "Vlandia" } }
            };
            var differentKingdomParty = new MobileParty
            {
                IsMainParty = false,
                ActualClan = new Clan { Kingdom = new Kingdom { Name = "Aserai" } }
            };

            Assert.False(ScopeExtension.IsOnlyEnnemies(mainParty));
            Assert.True(ScopeExtension.IsOnlyEnnemies(nullKingdomParty));
            Assert.False(ScopeExtension.IsOnlyEnnemies(sameKingdomParty));
            Assert.True(ScopeExtension.IsOnlyEnnemies(differentKingdomParty));
            Assert.False(ScopeExtension.IsOnlyEnnemies((MobileParty)null!));
        }

        [Fact]
        public void KingdomAndClanMethods_ReturnFalse_WhenMainHeroMissing()
        {
            Hero.MainHero = null;

            var clan = new Clan { Kingdom = new Kingdom { Name = "Vlandia" } };
            var kingdom = new Kingdom { Name = "Aserai" };

            Assert.False(ScopeExtension.IsOnlyPlayerClan(clan));
            Assert.False(ScopeExtension.IsOnlyPlayerKingdom(kingdom));
            Assert.False(ScopeExtension.IsOnlyEnnemies(kingdom));
        }
    }
}
