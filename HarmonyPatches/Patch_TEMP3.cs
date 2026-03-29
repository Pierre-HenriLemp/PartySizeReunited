using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace PartySizeReunited.HarmonyPatches
{
    //[HarmonyPatch(typeof(EncounterManager), nameof(EncounterManager.StartSettlementEncounter))]
    class Patch_TEMP3
    {
        public static bool Prefix(
            Settlement settlement,
            MobileParty attackerParty
            )
        {
            if (settlement.Party.MapEvent == null && settlement.IsUnderSiege && settlement.Parties.Any(e => e.Owner != null && e.Owner.IsHumanPlayerCharacter))
            {
                //Campaign.Current.SiegeEventManager.StartSiegeEvent(settlement, attackerParty);
            }
            return true;
        }
    }
}
