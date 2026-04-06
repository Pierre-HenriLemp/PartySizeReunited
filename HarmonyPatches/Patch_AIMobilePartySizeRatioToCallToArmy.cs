using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;

namespace PartySizeReunited.HarmonyPatches
{
    [HarmonyPatch(typeof(DefaultArmyManagementCalculationModel),
        nameof(DefaultArmyManagementCalculationModel.AIMobilePartySizeRatioToCallToArmy), MethodType.Getter)]
    public class Patch_AIMobilePartySizeRatioToCallToArmy
    {
        private const float VanillaValue = 0.6f;

        static bool Prefix(ref float __result)
        {
            __result = SubModule.PartySizeReunitedOptions.IsActivate && IsOptionSizeBonusExceedingVanilla()
                ? 0.4f
                : VanillaValue;

            return false;
        }

        /// <summary>
        /// Détermine si les bonus configurés dépassent un seuil qui justifierait de réduire le ratio de taille de la troupe mobile pour l'appel à l'armée.
        /// </summary>
        /// <returns>
        /// `true` si `FixedBonusAmnt` est supérieur à `40` ou si `PartyBonusAmnt` est supérieur à `1.2f`; sinon `false`.
        /// </returns>
        public static bool IsOptionSizeBonusExceedingVanilla()
        {
            var options = SubModule.PartySizeReunitedOptions;
            return options.FixedBonusAmnt > 40 || options.PartyBonusAmnt > 1.2f;
        }
    }
}