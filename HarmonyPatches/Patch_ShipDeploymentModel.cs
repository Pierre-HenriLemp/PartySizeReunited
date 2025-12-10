using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;

namespace PartySizeReunited.HarmonyPatches
{
    class Patch_ShipDeploymentModel
    {
        public static void TryApplyPatch(Harmony harmony)
        {
            try
            {
                // Chercher le type dans les assemblies chargés
                Type targetType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == "NavalDLCShipDeploymentModel");

                if (targetType == null)
                {
                    Console.WriteLine("NavalDLCShipDeploymentModel non trouvé, patch ignoré");
                    return;
                }

                // Obtenir la méthode à patcher
                MethodInfo originalMethod = targetType.GetMethod("GetShipDeploymentLimit",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (originalMethod == null)
                {
                    Console.WriteLine("Méthode GetShipDeploymentLimit non trouvée");
                    return;
                }

                // Obtenir la méthode de patch
                MethodInfo patchMethod = typeof(Patch_ShipDeploymentModel).GetMethod(nameof(Postfix),
                    BindingFlags.Public | BindingFlags.Static);

                // Appliquer le patch en Postfix
                harmony.Patch(originalMethod, postfix: new HarmonyMethod(patchMethod));

                Console.WriteLine("Patch NavalDLCShipDeploymentModel appliqué avec succès");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du patch NavalDLCShipDeploymentModel: {ex.Message}");
            }
        }

        public static void Postfix(object party, ref int __result)
        {
            try
            {
                // Accéder à party.LeaderHero.IsHumanPlayerCharacter via réflexion
                Type partyType = party.GetType();
                PropertyInfo leaderHeroProperty = partyType.GetProperty("LeaderHero");
                object? leaderHero = leaderHeroProperty?.GetValue(party);

                if (leaderHero != null)
                {
                    Type heroType = leaderHero.GetType();
                    PropertyInfo isHumanPlayerProperty = heroType.GetProperty("IsHumanPlayerCharacter");
                    bool isHumanPlayer = (bool)isHumanPlayerProperty.GetValue(leaderHero);

                    if (!isHumanPlayer && SubModule.WarSailsOptions.OnlyApplyToPlayer)
                    {
                        // Ne rien changer, garder le résultat original
                        return;
                    }
                    else
                    {
                        // Modifier le résultat
                        __result += SubModule.WarSailsOptions.BonusBoats;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur dans le patch ShipDeploymentModel: {ex.Message}");
            }
        }
    }
}