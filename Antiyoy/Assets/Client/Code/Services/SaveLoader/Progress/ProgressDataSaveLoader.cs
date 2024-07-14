using System.IO;
using ClientCode.Data;
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

        public ProjectProgressData LoadProject()
        {
            _saveLoader.Load(GetPath(StorageConstants.ProjectProgressKey), CreateDefaultProjectProgress(), out var data);
            return data;
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
                Load = _projectLoadData
            };
        }

        private MapProgressData LoadMapProgress(string key, MapProgressData defaultData)
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