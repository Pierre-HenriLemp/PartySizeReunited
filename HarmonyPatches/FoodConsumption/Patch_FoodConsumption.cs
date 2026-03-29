using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace PartySizeReunited.HarmonyPatches.FoodConsumption
{
    /// <summary>
    /// 
    /// </summary>
    [HarmonyPatch(typeof(DefaultMobilePartyFoodConsumptionModel), nameof(DefaultMobilePartyFoodConsumptionModel.CalculateDailyBaseFoodConsumptionf))]
    class Patch_FoodConsumption
    {
        static void Postfix(ref ExplainedNumber __result)
        {
            if (!ShouldApplyPatch())
            {
                return;
            }

            // Reduce by 90% the result.
            __result.AddFactor(-0.9f, new("Party Size Reunited food consumption reduction"));
        }

        private static bool ShouldApplyPatch()
        {
            var options = SubModule.PartySizeReunitedOptions;
            if (!options.IsActivate)
            {
                return false;
            }

            return options.NoMoreSupplyIssues;
        }
    }
}
