using HarmonyLib;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using PartySizeReunited.HarmonyPatches;
using PartySizeReunited.McMMenu;
using PartySizeReunited.McMMenu.Options;
using PartySizeReunited.Services;
using TaleWorlds.Core;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;

namespace PartySizeReunited
{
    public class SubModule : MBSubModuleBase
    {
        public const string ModuleId = "PartySizeReunited";

        public static WarSailsOptions WarSailsOptions = new();
        public static PartySizeReunitedOptions PartySizeReunitedOptions = new();
        public static CompanionsOptions CompanionsOptions = new();
        public static PartyRecruitmentOptions partyRecruitmentOptions = new();
        public static bool isWarSailsModulePresent = false;

        private static readonly Harmony harmony = new(ModuleId);
        private static FluentGlobalSettings? settings;

        private bool initOnce = false;

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            // Découvrir tous les modèles de taille de party
            var discoveryService = new PartySizeModelDiscoveryService();
            var partySizeModels = discoveryService.DiscoverPartySizeModels();

            // Injecter les modèles dans notre PartySize
            gameStarterObject.AddModel(new PartySize(partySizeModels));
            harmony.PatchAll();
        }

        public override void OnInitialState()
        {
            base.OnInitialState();

            if (initOnce)
            {
                return;
            }

            initOnce = true;

            isWarSailsModulePresent = ModuleHelper.GetActiveModules()
                .Exists(module =>
                    module.IsOfficial &&
                    module.Category == ModuleCategory.Singleplayer &&
                    module.Id == "NavalDLC"
                );

            ISettingsBuilder builder = McMSettings.InitMcMSettings();
            McMPartySizeReunitedSettings.AddPartySizeSettings(builder, PartySizeReunitedOptions);
            AddMoreSettings(builder);
            AddPartyAndCompanionSettings(builder);
            settings = builder.BuildAsGlobal();
            settings?.Register();
        }

        private void AddMoreSettings(ISettingsBuilder builder)
        {
            if (isWarSailsModulePresent)
            {
                // Add WarSails mod options
                McMWarSailsSettings.AddWarsailsSettings(builder, WarSailsOptions);

                // Patch Warsails deployment method
                Patch_ShipDeploymentModel.TryApplyPatch(harmony);
            }
        }

        private void AddPartyAndCompanionSettings(ISettingsBuilder builder)
        {
            McMCompanionSettings.AddCompanionsSettings(builder, CompanionsOptions);
            McMPartyRecruitmentSettings.AddPartyRecruitmentSettings(builder, partyRecruitmentOptions);
        }
    }
}

