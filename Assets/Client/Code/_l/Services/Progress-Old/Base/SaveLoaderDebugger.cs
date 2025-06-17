// ReSharper disable UnusedParameter.Local

namespace ClientCode.Services.Progress.Base
{
    public static class SaveLoaderDebugger
    {
        public static void DebugLoadMap(string key)
        {
#if DEBUG_SAVE_LOADER
            UnityEngine.Debug.Log($"Load map: {key}");
#endif
        }

        public static void DebugSaveMap(string key)
        {
#if DEBUG_SAVE_LOADER
            UnityEngine.Debug.Log($"Save map: {key}");
#endif
        }

        public static void DebugRemoveMap(string key)
        {
#if DEBUG_SAVE_LOADER
            UnityEngine.Debug.Log($"Remove map: {key}");
#endif
        }

        public static void DebugLoadProject()
        {
#if DEBUG_SAVE_LOADER
            UnityEngine.Debug.Log("Load proj-progress");
#endif
        }

        public static void DebugSaveProject()
        {
#if DEBUG_SAVE_LOADER
            UnityEngine.Debug.Log("Save proj-progress");
#endif
        }
    }
}