using System.IO;
using ClientCode.Data.Const;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Load;
using ClientCode.Data.Static;
using ClientCode.Services.ProgressDataProvider;

namespace ClientCode.Services.SaveLoader.Progress
{
    public class ProgressDataSaveLoader : IProgressDataSaveLoader
    {
        private readonly ISaveLoader _saveLoader;
        private readonly IProgressDataProvider _dataProvider;
        private readonly ProjectLoadData _projectLoadData;

        public ProgressDataSaveLoader(ISaveLoader saveLoader, IProgressDataProvider dataProvider, ProjectLoadData projectLoadData)
        {
            _saveLoader = saveLoader;
            _dataProvider = dataProvider;
            _projectLoadData = projectLoadData;
        }

        public ProjectLoadData LoadProjectLoadData() => _projectLoadData;

        public bool LoadProjectProgress(ProjectProgressData defaultData)
        {
            var result = _saveLoader.Load(GetPath(StorageConstants.ProjectProgressKey), defaultData, out var data);
            _dataProvider.Project = data;
            return result;
        }

        public bool LoadMapProgress(string key, MapProgressData defaultData)
        {
            var result = _saveLoader.Load(GetPath(key, StorageConstants.MapSubPath), defaultData, out var data);
            _dataProvider.Map = data;
            return result;
        }

        private string GetPath(string key, string subPath = null)
        {
            var path = StorageConstants.BasePath;

            if (subPath != null)
                path = Path.Combine(StorageConstants.BasePath, subPath);
            
            UnityEngine.Debug.LogError(Path.Combine(path, key) + StorageConstants.FilesExtension);
            
            return Path.Combine(path, key) + StorageConstants.FilesExtension;
        }
    }
}