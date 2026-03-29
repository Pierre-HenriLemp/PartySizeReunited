using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace PartySizeReunited.HarmonyPatches
{
    [HarmonyPatch(typeof(Settlement), "OnPartyInteraction")]
    class Patch_TEMP5
    {

        [HarmonyPrefix]
        private static bool Prefix(Settlement __instance, MobileParty engagingParty)
        {
            if (engagingParty.ShortTermTargetSettlement != null && ((engagingParty.IsCurrentlyAtSea && engagingParty.IsTargetingPort) || ((engagingParty.ShortTermTargetSettlement.Party.SiegeEvent == null || engagingParty == MobileParty.MainParty || engagingParty.MapFaction == engagingParty.ShortTermTargetSettlement.SiegeEvent.BesiegerCamp.MapFaction) && (engagingParty.ShortTermTargetSettlement.Party.MapEvent == null || engagingParty == MobileParty.MainParty || engagingParty.MapFaction == engagingParty.ShortTermTargetSettlement.Party.MapEvent.AttackerSide.LeaderParty.MapFaction || (engagingParty.ShortTermTargetSettlement.Party.MapEvent.IsSallyOut && engagingParty.MapFaction == engagingParty.ShortTermTargetSettlement.Party.MapEvent.DefenderSide.LeaderParty.MapFaction)))))
            {
                if (engagingParty == MobileParty.MainParty && (engagingParty.ShortTermTargetSettlement.Party.MapEvent == null || !engagingParty.ShortTermTargetSettlement.Party.MapEvent.IsRaid || engagingParty.ShortTermTargetSettlement.Party.MapEvent.DefenderSide.NumRemainingSimulationTroops > 0))
                {
                    (Game.Current.GameStateManager.ActiveState as MapState)?.OnMainPartyEncounter();
                }
                if ((engagingParty.ShortTermTargetSettlement.Party.MapEvent == null || engagingParty.ShortTermTargetSettlement.Party.MapEvent != null && engagingParty.ShortTermTargetSettlement.Party.MapEvent.IsRaid) && engagingParty.DefaultBehavior == AiBehavior.RaidSettlement)
                {
                    engagingParty.Ai.RethinkAtNextHourlyTick = true;
                    engagingParty.SetMoveModeHold();
                }
                else
                {
                    if (__instance.IsUnderSiege && __instance.SiegeEvent != null && __instance.SiegeEvent.IsPlayerSiegeEvent && !__instance.SiegeEvent.BesiegerCamp.IsReadyToBesiege)
                    {
                        //engagingParty.Ai.RethinkAtNextHourlyTick = true;
                        //engagingParty.SetMoveModeHold();
                        return false;
                    }
                    EncounterManager.StartSettlementEncounter(engagingParty, engagingParty.ShortTermTargetSettlement);
                }
            }

            return false;
        }
    }
}
