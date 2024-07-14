using UnityEngine;

namespace ClientCode.Data.Const
{
    public static class StorageConstants
    {
        public const string MapSubPath = "Maps";
        public const string FilesExtension = "data";

        public const string ProjectProgressKey = "ProjectProgressData";
        public static readonly string BasePath = $"{Application.dataPath}/Saves";
    }
}