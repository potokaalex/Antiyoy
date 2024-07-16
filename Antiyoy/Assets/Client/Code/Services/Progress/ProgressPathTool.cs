using System.IO;
using ClientCode.Data.Static.Const;

namespace ClientCode.Services.Progress
{
    public static class ProgressPathTool
    {
        public static string GetFilePath(string key, string subPath = null) =>
            Path.ChangeExtension(Path.Combine(GetPath(subPath), key), StorageConstants.FilesExtension);

        public static string GetPath(string subPath)
        {
            var path = StorageConstants.BasePath;

            if (subPath != null)
                path = Path.Combine(StorageConstants.BasePath, subPath);

            return path;
        }
    }
}