using HarmonyLib;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;

namespace PartySizeReunited.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerEncounter), nameof(PlayerEncounter.CheckIfBattleShouldContinueAfterBattleMission))]
    class Patch_TEMP
    {
        public static void Postfix(ref bool __result, ref MapEvent ____mapEvent)
        {
            var attacker = ____mapEvent.AttackerSide;
            var defender = ____mapEvent.DefenderSide;
            var attackerStrength = attacker.RecalculateStrengthOfSide();
            var defenderStrength = defender.RecalculateStrengthOfSide();
            __result = attackerStrength >= defenderStrength * 0.5;
        }
    }
}
