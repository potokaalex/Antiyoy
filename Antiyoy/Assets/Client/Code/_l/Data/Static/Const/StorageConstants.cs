using UnityEngine;

namespace ClientCode.Data.Static.Const
{
    public static class StorageConstants
    {
        public static readonly string BasePath = $"{Application.dataPath}/Saves";
        public const string MapSubPath = "Maps";
        public const string FilesExtension = "data";
    }
}