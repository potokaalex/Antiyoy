using Client.Code.Utilities.Editor.Base;
using UnityEditor;

namespace Client.Code.Utilities.Editor
{
    public static class SaveLoaderDebugToggle
    {
        private const string MenuItemPath = "Tools/Debug/DEBUG_SAVE_LOADER";
        private const string Symbol = "DEBUG_SAVE_LOADER";

        [MenuItem(MenuItemPath)]
        private static void ToggleDebugSymbol() => DefineSymbolsToggleTool.ToggleDebugSymbol(Symbol);

        [MenuItem(MenuItemPath, true)]
        private static bool ToggleDebugSymbolValidate() => DefineSymbolsToggleTool.ToggleDebugSymbolValidate(Symbol, MenuItemPath);
    }
}