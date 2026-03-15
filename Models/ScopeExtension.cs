using System.ComponentModel;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;

namespace PartySizeReunited.Models
{
    public class ScopeExtension
    {
        public IScope Scope { get; }

        public ScopeExtension(IScope scope)
        {
            Scope = scope;
        }

        public override string ToString()
        {
            var field = Scope.GetType().GetField(Scope.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? Scope.ToString();
        }

        public static bool IsEveryoneExceptPlayer(PartyBase party)
        {
            return !IsOnlyPlayer(party);
        }

        public static bool IsOnlyPlayer(PartyBase party)
        {
            return party.LeaderHero.IsHumanPlayerCharacter == true;
        }

        public static bool IsOnlyEnnemies(PartyBase party)
        {
            return !party.LeaderHero.IsHumanPlayerCharacter &&
                        party.LeaderHero.Clan?.MapFaction != null &&
                        Hero.MainHero?.Clan?.MapFaction != null &&
                        party.LeaderHero.Clan.MapFaction.Name != Hero.MainHero.Clan.MapFaction.Name;
        }

        public static bool IsOnlyEnnemies(IFaction faction)
        {
            return faction != null &&
                        Hero.MainHero?.Clan?.MapFaction != null &&
                        faction.Name != Hero.MainHero.Clan.MapFaction.Name;
        }

        public static bool IsOnlyPlayerClan(PartyBase party)
        {
            return !party.LeaderHero.IsHumanPlayerCharacter &&
                        party.LeaderHero.Clan != null &&
                        Hero.MainHero?.Clan != null &&
                        party.LeaderHero.Clan == Hero.MainHero.Clan;
        }

        public static bool IsOnlyPlayerClan(Clan clan)
        {
            return clan != null &&
                        Hero.MainHero?.Clan != null &&
                        clan == Hero.MainHero?.Clan;
        }

        public static bool IsOnlyPlayerFaction(PartyBase party)
        {
            return !party.LeaderHero.IsHumanPlayerCharacter &&
                        party.LeaderHero.Clan?.MapFaction != null &&
                        Hero.MainHero?.Clan?.MapFaction != null &&
                        party.LeaderHero.Clan.MapFaction.Name == Hero.MainHero.Clan.MapFaction.Name;
        }

        public static bool IsOnlyPlayerFaction(IFaction faction)
        {
            return faction != null &&
                        Hero.MainHero?.Clan?.MapFaction != null &&
                        faction?.Name == Hero.MainHero?.Clan.MapFaction.Name;
        }

        public static bool IsEveryoneExceptPlayer(MobileParty party)
        {
            return !IsOnlyPlayer(party);
        }

        public static bool IsOnlyPlayer(MobileParty party)
        {
            return party.LeaderHero?.IsHumanPlayerCharacter == true;
        }

        public static bool IsOnlyEnnemies(MobileParty party)
        {
            return !party.LeaderHero.IsHumanPlayerCharacter &&
                        party.LeaderHero.Clan?.MapFaction != null &&
                        Hero.MainHero?.Clan?.MapFaction != null &&
                        party.LeaderHero.Clan.MapFaction.Name != Hero.MainHero.Clan.MapFaction.Name;
        }

        public static bool IsOnlyPlayerClan(MobileParty party)
        {
            return !party.LeaderHero.IsHumanPlayerCharacter &&
                        party.LeaderHero.Clan != null &&
                        Hero.MainHero?.Clan != null &&
                        party.LeaderHero.Clan == Hero.MainHero.Clan;
        }

        public static bool IsOnlyPlayerFaction(MobileParty party)
        {
            return !party.LeaderHero.IsHumanPlayerCharacter &&
                        party.LeaderHero.Clan?.MapFaction != null &&
                        Hero.MainHero?.Clan?.MapFaction != null &&
                        party.LeaderHero.Clan.MapFaction.Name == Hero.MainHero.Clan.MapFaction.Name;
        }
    }

    public enum IScope
    {
        [Description("Everyone (Except player)")]
        Everyone,

        [Description("Only player")]
        Only_player,

        [Description("Only player clan")]
        Only_player_clan,

        [Description("Only player faction")]
        Only_player_faction,

        [Description("Only enemies")]
        Only_ennemies
    }
}
