using System.IO;
using ClientCode.Data.Const;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Load;

namespace ClientCode.Services.SaveLoader.Progress
{
    public class ProgressDataSaveLoader : IProgressDataSaveLoader
    {
        private readonly ProjectLoadData _projectLoadData;
        private readonly ISaveLoader _saveLoader;

        public ProgressDataSaveLoader(ISaveLoader saveLoader, ProjectLoadData projectLoadData)
        {
            _saveLoader = saveLoader;
            _projectLoadData = projectLoadData;
        }

        public ProjectLoadData LoadProjectLoadData() => _projectLoadData;

        public ProjectProgressData LoadProjectProgress(ProjectProgressData defaultData)
        {
            _saveLoader.Load(GetPath(StorageConstants.ProjectProgressKey), defaultData, out var data);
            return data;
        }

        public MapProgressData LoadMapProgress(string key, MapProgressData defaultData)
        {
            _saveLoader.Load(GetPath(key, StorageConstants.MapSubPath), defaultData, out var data);
            return data;
        }

        private string GetPath(string key, string subPath = null)
        {
            var path = StorageConstants.BasePath;

            if (subPath != null)
                path = Path.Combine(StorageConstants.BasePath, subPath);

            return Path.Combine(path, key) + StorageConstants.FilesExtension;
        }
    }
}