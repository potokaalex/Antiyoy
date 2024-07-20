using ClientCode.Utilities.Editor.Base;
using UnityEditor;

namespace ClientCode.Utilities.Editor
{
    public static class ProjectDebugToggle
    {
        private const string MenuItemPath = "Tools/Debug/DEBUG_PROJECT";
        private const string Symbol = "DEBUG_PROJECT";

        [MenuItem(MenuItemPath)]
        private static void ToggleDebugSymbol() => DefineSymbolsToggleTool.ToggleDebugSymbol(Symbol);

        [MenuItem(MenuItemPath, true)]
        private static bool ToggleDebugSymbolValidate() => DefineSymbolsToggleTool.ToggleDebugSymbolValidate(Symbol, MenuItemPath);
    }
}