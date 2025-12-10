using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;

namespace PartySizeReunited.HarmonyPatches
{
    [HarmonyPatch(typeof(DefaultArmyManagementCalculationModel), nameof(DefaultArmyManagementCalculationModel.CalculatePartyInfluenceCost))]
    class Patch_DefaultArmyManagementCalculationModel
    {
        static void Postfix(ref int __result)
        {
            // Multiply result of CalculatePartyInfluenceCost that return the cost of influence by the setting PartyInfluenceCost that is a float percentage
            __result = SubModule.PartySizeReunitedOptions.IsActivate ?
                (int)(SubModule.PartySizeReunitedOptions.PartyInfluenceCost * __result) :
                __result;
        }
    }
}
