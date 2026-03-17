namespace TaleWorlds.CampaignSystem
{
    public class Hero
    {
        public static Hero? MainHero { get; set; }
        public bool IsHumanPlayerCharacter { get; set; }
        public Clan? Clan { get; set; }
    }

    public class Clan
    {
        public Kingdom? Kingdom { get; set; }
    }

    public class Kingdom
    {
        public string Name { get; set; } = string.Empty;
    }
}

namespace TaleWorlds.CampaignSystem.Party
{
    using TaleWorlds.CampaignSystem;

    public class PartyBase
    {
        public Hero? Owner { get; set; }
    }

    public class MobileParty
    {
        public bool IsMainParty { get; set; }
        public Clan? ActualClan { get; set; }
    }
}
