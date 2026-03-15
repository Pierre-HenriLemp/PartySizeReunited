using HarmonyLib;
using PartySizeReunited.Services;
using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace PartySizeReunited.HarmonyPatches
{
    class Patch_NavalDLC_PlayerPartyNavalSpeed
    {
        public static void TryApplyPatch(Harmony harmony)
        {
            try
            {
                // Chercher le type dans les assemblies chargés
                Type targetType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == "NavalDLCPartySpeedCalculationModel");

                if (targetType == null)
                {
                    Utils.PrintError("NavalDLCPartySpeedCalculationModel non trouvé, patch ignoré");
                    return;
                }

                // Obtenir la méthode à patcher
                MethodInfo originalMethod = targetType.GetMethod("CalculateNavalBaseSpeed",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (originalMethod == null)
                {
                    Utils.PrintError("Méthode CalculateNavalBaseSpeed non trouvée");
                    return;
                }

                // Obtenir la méthode de patch
                MethodInfo patchMethod = typeof(Patch_NavalDLC_PlayerPartyNavalSpeed).GetMethod(nameof(Postfix),
                    BindingFlags.Public | BindingFlags.Static);

                // Appliquer le patch en Postfix
                harmony.Patch(originalMethod, postfix: new HarmonyMethod(patchMethod));
            }
            catch (Exception ex)
            {
                Utils.PrintError($"Erreur lors du patch NavalDLCPartySpeedCalculationModel: {ex.Message}");
            }
        }

        public static void Postfix(MobileParty mobileParty, ref ExplainedNumber __result)
        {
            if (mobileParty.Party.LeaderHero != null && !mobileParty.Party.LeaderHero.IsHumanPlayerCharacter)
            {
                return;
            }

            var options = SubModule.PartySizeReunitedOptions;
            var currentGarrisonSize = __result.ResultNumber;
            TextObject description = new("PartySizeReunited modifier");
            if (options.PlayerPartySpeedFixedBonus != 0)
            {
                __result.Add(
                    options.PlayerPartySpeedFixedBonus,
                    description
                );
            }
            else
            {
                __result.AddFactor(
                    options.PlayerPartySpeedMultiBonus,
                    description
                    );
            }
        }
    }
}
