using UnityEngine;

namespace ClientCode.Data.Const
{
    public static class StorageConstants
    {
        public static readonly string BasePath = $"{Application.dataPath}/Saves";
        public const string MapSubPath = "Maps";
        public const string FilesExtension = ".dat";
        
        public const string ProjectProgressKey = "ProjectProgressData";
    }
}