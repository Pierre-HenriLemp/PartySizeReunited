using HarmonyLib;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;

namespace PartySizeReunited.HarmonyPatches
{
    //[HarmonyPatch(typeof(PlayerEncounter), "ContinueBattle")]
    class Patch_TEMP2
    {
        static readonly Action<MapEventSide> _commitXpGains =
            AccessTools.MethodDelegate<Action<MapEventSide>>(
                AccessTools.Method(typeof(MapEventSide), "CommitXpGains"));

        static readonly Action<MapEvent> _applyRenownAndInfluenceChanges =
            AccessTools.MethodDelegate<Action<MapEvent>>(
                AccessTools.Method(typeof(MapEvent), "ApplyRenownAndInfluenceChanges"));

        public static bool Prefix(
            PlayerEncounter __instance,
            ref MapEvent ____mapEvent,
            ref CampaignBattleResult ____campaignBattleResult,
            ref bool ____stateHandled,
            ref PlayerEncounterState ____mapEventState)
        {
            MapEventSide mapEventSide = ____mapEvent.GetMapEventSide(____mapEvent.PlayerSide);
            MapEventSide otherSide = mapEventSide.OtherSide;
            ____mapEvent.RecalculateStrengthOfSides();
            if (____mapEvent.IsNavalMapEvent && otherSide.Parties.Sum((MapEventParty x) => x.Ships.Count) == 0)
            {
                ____mapEvent?.SetOverrideWinner(____mapEvent.PlayerSide);
                PlayerEncounter.EnemySurrender = true;
                ____mapEventState = PlayerEncounterState.PrepareResults;
            }
            else if (____mapEvent.IsNavalMapEvent && mapEventSide.Parties.Sum((MapEventParty x) => x.Ships.Count) == 0)
            {
                ____mapEvent?.SetOverrideWinner(otherSide.MissionSide);
                ____mapEventState = PlayerEncounterState.PrepareResults;
            }
            else
            {
                _commitXpGains(____mapEvent.AttackerSide);
                _commitXpGains(____mapEvent.DefenderSide);
                _applyRenownAndInfluenceChanges(____mapEvent);
                ____mapEvent.SetOverrideWinner(BattleSideEnum.None);
                if (____mapEvent.IsSiegeAssault && otherSide == ____mapEvent.AttackerSide)
                {
                    CampaignBattleResult campaignBattleResult = ____campaignBattleResult;
                    if (campaignBattleResult != null && campaignBattleResult.EnemyRetreated)
                    {
                        ____mapEvent.AttackerSide.Parties.ToList();
                        ____mapEvent.FinishBattleAndKeepSiegeEvent();
                        // _mapEvent = null; -- removed intentionally
                        GameMenu.ActivateGameMenu("menu_siege_strategies");
                    }
                }
                ____campaignBattleResult = null;
                ____stateHandled = true;
            }
            return false; // skip original
        }
    }
}
