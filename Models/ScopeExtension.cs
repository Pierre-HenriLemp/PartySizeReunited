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
            return party.Owner != null && party.Owner.IsHumanPlayerCharacter == true;
        }

        public static bool IsOnlyEnnemies(PartyBase party)
        {
            return party.Owner == null ||
                !party.Owner.IsHumanPlayerCharacter ||
                party.Owner.Clan == null ||
                party.Owner.Clan?.Kingdom == null ||
                (Hero.MainHero?.Clan?.Kingdom != null && party.Owner.Clan.Kingdom.Name != Hero.MainHero.Clan.Kingdom.Name) ||
                (Hero.MainHero?.Clan != null && party.Owner.Clan != Hero.MainHero?.Clan);
        }

        public static bool IsOnlyEnnemies(Kingdom kingdom)
        {
            return kingdom != null &&
                        Hero.MainHero?.Clan?.Kingdom != null &&
                        kingdom.Name != Hero.MainHero.Clan.Kingdom.Name;
        }

        public static bool IsOnlyPlayerClan(PartyBase party)
        {
            return party.Owner != null &&
                !party.Owner.IsHumanPlayerCharacter &&
                        party.Owner.Clan != null &&
                        Hero.MainHero?.Clan != null &&
                        party.Owner.Clan == Hero.MainHero.Clan;
        }

        public static bool IsOnlyPlayerClan(Clan clan)
        {
            return clan != null &&
                        Hero.MainHero?.Clan != null &&
                        clan == Hero.MainHero?.Clan;
        }

        public static bool IsOnlyPlayerKingdom(PartyBase party)
        {
            return party.Owner != null &&
                !party.Owner.IsHumanPlayerCharacter &&
                        party.Owner.Clan?.Kingdom != null &&
                        Hero.MainHero?.Clan?.Kingdom != null &&
                        party.Owner.Clan.Kingdom.Name == Hero.MainHero.Clan.Kingdom.Name;
        }

        public static bool IsOnlyPlayerKingdom(Kingdom kingdom)
        {
            return kingdom != null &&
                        Hero.MainHero?.Clan?.Kingdom != null &&
                        kingdom == Hero.MainHero?.Clan?.Kingdom;
        }

        public static bool IsEveryoneExceptPlayer(MobileParty party)
        {
            return !IsOnlyPlayer(party);
        }

        public static bool IsOnlyPlayer(MobileParty party)
        {
            return party.IsMainParty == true;
        }

        public static bool IsOnlyEnnemies(MobileParty party)
        {
            return !party.IsMainParty && (
                        party.ActualClan?.Kingdom == null ||
                        (
                        Hero.MainHero?.Clan?.Kingdom != null &&
                        party.ActualClan.Kingdom.Name != Hero.MainHero.Clan.Kingdom.Name
                        )
                        );
        }

        public static bool IsOnlyPlayerClan(MobileParty party)
        {
            return !party.IsMainParty &&
                        party.ActualClan != null &&
                        Hero.MainHero?.Clan != null &&
                        party.ActualClan == Hero.MainHero.Clan;
        }

        public static bool IsOnlyPlayerKingdom(MobileParty party)
        {
            return !party.IsMainParty &&
                        party.ActualClan?.Kingdom != null &&
                        Hero.MainHero?.Clan?.Kingdom != null &&
                        party.ActualClan.Kingdom.Name == Hero.MainHero.Clan.Kingdom.Name;
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

        [Description("Only player kingdom")]
        Only_player_kingdom,

        [Description("Only enemies")]
        Only_ennemies
    }
}
