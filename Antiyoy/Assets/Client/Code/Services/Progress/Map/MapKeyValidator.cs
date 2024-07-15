using ClientCode.Data.Static.Const;
using ClientCode.Services.Progress.Base;

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