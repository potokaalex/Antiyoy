using System.IO;
using ClientCode.Data.Const;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;

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

        public ProjectProgressData LoadProject()
        {
            //_saveLoader.Load(GetPath(StorageConstants.ProjectProgressKey), CreateDefaultProjectProgress(), out var data);
            return CreateDefaultProjectProgress();
        }

        public MainMenuProgressData LoadMainMenu() => new();

        public MapEditorProgressData LoadMapEditor(string mapKey)
        {
            return new MapEditorProgressData
            {
                Map = LoadMapProgress(mapKey, new MapProgressData())
            };
        }

        private ProjectProgressData CreateDefaultProjectProgress()
        {
            return new ProjectProgressData
            {
                Load = _projectLoadData,
                MapKeys = _saveLoader.GetFileNames(GetPath(StorageConstants.MapSubPath), StorageConstants.FilesExtension)
            };
        }

        private MapProgressData LoadMapProgress(string key, MapProgressData defaultData)
        {
            if (string.IsNullOrEmpty(key))
                return defaultData;
            
            _saveLoader.Load(GetFilePath(key, StorageConstants.MapSubPath), defaultData, out var data);
            return data;
        }

        private string GetFilePath(string key, string subPath = null) =>
            Path.ChangeExtension(Path.Combine(GetPath(subPath), key), StorageConstants.FilesExtension);

        private string GetPath(string subPath)
        {
            var path = StorageConstants.BasePath;

            if (subPath != null)
                path = Path.Combine(StorageConstants.BasePath, subPath);

            return path;
        }
    }
}