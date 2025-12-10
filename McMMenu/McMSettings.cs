using MCM.Abstractions.FluentBuilder;

namespace PartySizeReunited.McMMenu
{
    internal static class McMSettings
    {
        private static string SettingsId => "PartySizeReunited";
        private static string DisplayedName => "Party Size Reunited";

        public static ISettingsBuilder InitMcMSettings()
        {
            return BaseSettingsBuilder.Create(SettingsId, DisplayedName)!
                .SetFormat("json")
                .SetFolderName("PartySizeReunited");
        }
    }
}
