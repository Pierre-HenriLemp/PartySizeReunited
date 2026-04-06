using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using PartySizeReunited.Services;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using TaleWorlds.CampaignSystem.Party;

namespace PartySizeReunited.HarmonyPatches
{
    [HarmonyPatch(typeof(AiMilitaryBehavior), "AiHourlyTick")]
    public class Patch_AiMilitaryBehavior_AiHourlyTick_PartySizeThreshold
    {
        private const float VanillaThreshold = 0.6f;
        private const float NewThreshold = 0.4f;
        
        private static bool _isWarningAlreadyDisplayed;

        /// <summary>
        /// Harmony transpiler qui parcourt l'IL de <c>AiMilitaryBehavior.AiHourlyTick</c>
        /// et remplace la constante <c>0.6f</c> (seuil vanilla après <c>MobileParty.PartySizeRatio</c>)
        /// par un appel à <see cref="GetArmyCallThreshold"/> afin d'utiliser un seuil dynamique.
        /// Si le motif IL n'est pas trouvé, aucun remplacement n'est appliqué.
        /// Ce patch empêche le debug de la method origniel tant qu'il est actif.
        /// </summary>
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var getPartySizeRatio = AccessTools.PropertyGetter(typeof(MobileParty), nameof(MobileParty.PartySizeRatio));
            var getThresholdMethod = AccessTools.Method(
                typeof(Patch_AiMilitaryBehavior_AiHourlyTick_PartySizeThreshold),
                nameof(GetArmyCallThreshold));

            bool replaced = false;

            var list = new List<CodeInstruction>(instructions);

            for (int i = 0; i < list.Count - 1; i++)
            {
                // On repère l'accès à mobileParty.PartySizeRatio puis le ldc.r4 0.6 juste après.
                if (list[i].Calls(getPartySizeRatio) &&
                    list[i + 1].opcode == OpCodes.Ldc_R4 &&
                    list[i + 1].operand is float f &&
                    f > 0.5999f && f < 0.6001f)
                {
                    var old = list[i + 1];
                    var replacement = new CodeInstruction(OpCodes.Call, getThresholdMethod);

                    // Important : conserver métadonnées IL
                    replacement.labels.AddRange(old.labels);
                    replacement.blocks.AddRange(old.blocks);

                    list[i + 1] = replacement;
                    replaced = true;
                    break;
                }
            }

            if (!replaced && !_isWarningAlreadyDisplayed)
            {
                _isWarningAlreadyDisplayed = true;
                Utils.Print(
                    "[PartySizeReunited] AiMilitaryBehavior threshold patch NOT applied. Pattern not found. AI armies may not correctly work.");
            }

            return list;
        }

        private static float GetArmyCallThreshold()
        {
            return SubModule.PartySizeReunitedOptions.IsActivate &&
                   Patch_AIMobilePartySizeRatioToCallToArmy.IsOptionSizeBonusExceedingVanilla()
                ? NewThreshold
                : VanillaThreshold;
        }
    }
}