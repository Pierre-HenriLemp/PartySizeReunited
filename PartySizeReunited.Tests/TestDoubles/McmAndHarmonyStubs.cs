using System.Reflection;

namespace MCM.Common
{
    public class Dropdown<T>
    {
        public Dropdown(T[] values, int selectedIndex)
        {
            Values = values;
            SelectedIndex = selectedIndex;
        }

        public T[] Values { get; }
        public int SelectedIndex { get; }
        public T SelectedValue => Values[SelectedIndex];
    }
}

namespace HarmonyLib
{
    public class Harmony
    {
        public void Patch(MethodInfo original, HarmonyMethod? prefix = null, HarmonyMethod? postfix = null)
        {
        }
    }

    public class HarmonyMethod : Attribute
    {
        public HarmonyMethod(MethodInfo methodInfo)
        {
        }
    }
}
