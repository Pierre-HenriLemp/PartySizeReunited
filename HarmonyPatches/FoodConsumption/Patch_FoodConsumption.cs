using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace PartySizeReunited.HarmonyPatches.FoodConsumption
{
    /// <summary>
    /// 
    /// </summary>
    [HarmonyPatch(typeof(DefaultMobilePartyFoodConsumptionModel), nameof(DefaultMobilePartyFoodConsumptionModel.CalculateDailyBaseFoodConsumptionf))]
    class Patch_FoodConsumption
    {
        static void Postfix(MobileParty party, ref ExplainedNumber __result)
        {
            if (!ShouldApplyPatch(party))
            {
                return;
            }

            // Reduce by 70% the result.
            __result.AddFactor(-0.7f, new("Party Size Reunited food consumption reduction"));
        }

        private static bool ShouldApplyPatch(MobileParty party)
        {
            var options = SubModule.PartySizeReunitedOptions;
            if (!options.IsActivate || party.Owner != null && party.Owner.IsHumanPlayerCharacter)
            {
                return false;
            }

            return options.NoMoreSupplyIssues;
        }
    }
}
