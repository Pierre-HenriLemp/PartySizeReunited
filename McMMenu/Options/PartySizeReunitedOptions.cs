using MCM.Common;
using PartySizeReunited.Models;

namespace PartySizeReunited.McMMenu.Options
{
    public class PartySizeReunitedOptions
    {
        public bool IsPlayerPartyImpacted { get; set; }
        public bool NoMoreSupplyIssues { get; set; }
        public float PartyBonusAmnt { get; set; } = 1f;
        public float PartyInfluenceCost { get; set; } = 1f;
        public int FixedBonusAmnt { get; set; } = 0;
        public Dropdown<ScopeExtension> BonusScope { get; set; } = new Dropdown<ScopeExtension>(new ScopeExtension[]
        {
            new (IScope.Everyone),
            new (IScope.Only_player),
            new (IScope.Only_player_clan),
            new (IScope.Only_player_faction),
            new (IScope.Only_ennemies)
        }, 0);
    }
}
