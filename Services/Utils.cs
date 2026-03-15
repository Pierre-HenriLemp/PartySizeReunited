using TaleWorlds.Library;

namespace PartySizeReunited.Services
{
    static class Utils
    {
        public static void Print(string message, float red = 1, float green = 1, float blue = 1, float alpha = 1)
        {
            InformationManager.DisplayMessage(new InformationMessage(message, new Color(red, green, blue, alpha)));
        }

        public static void PrintError(string message, float red = 1, float green = 0, float blue = 0, float alpha = 0)
        {
            InformationManager.DisplayMessage(new InformationMessage(message, new Color(red, green, blue, alpha)));
        }
    }
}
