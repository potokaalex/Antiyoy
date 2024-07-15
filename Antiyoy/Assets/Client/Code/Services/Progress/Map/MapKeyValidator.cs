using ClientCode.Data.Const;
using ClientCode.Services.SaveLoad;

namespace ClientCode.Services.Progress.Map
{
    public static class MapKeyValidator
    {
        public static bool IsValid(string key)
        {
            var path = ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath);
            return SaveLoader.IsValidSavePath(path);
        }
    }
}