using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement;
using TaleWorlds.Localization;

namespace PartySizeReunited.HarmonyPatches
{
    [HarmonyPatch(typeof(ArmyManagementItemVM), nameof(ArmyManagementItemVM.UpdateEligibility))]
    class Patch_ArmyManagementItemVM
    {
        static void Postfix(ArmyManagementItemVM __instance)
        {
            // If current party has more than 99 troops, then allow to recruit it
            int partyCurrentSize = __instance.Party.MemberRoster.TotalManCount;

            if (partyCurrentSize >= 100)
            {
                var fieldRef = AccessTools.FieldRefAccess<ArmyManagementItemVM, TextObject>("_eligibilityReason");
                fieldRef(__instance) = TextObject.GetEmpty();
                __instance.IsEligible = true;
            }

        }
    }
}
