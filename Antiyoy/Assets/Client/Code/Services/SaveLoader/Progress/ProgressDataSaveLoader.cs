using System.IO;
using ClientCode.Data.Const;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.SaveLoader.Base;

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
            return new ProjectProgressData
            {
                Load = _projectLoadData
            };
        }

        public MainMenuProgressData LoadMainMenu()
        {
            return new MainMenuProgressData
            {
                MapKeys = _saveLoader.GetFileNames(GetPath(StorageConstants.MapSubPath), StorageConstants.FilesExtension)
            };
        }

        public MapEditorProgressData LoadMapEditor(string mapKey)
        {
            return new MapEditorProgressData
            {
                Map = LoadMap(mapKey)
            };
        }

        public bool SaveMapEditor(MapEditorProgressData progress)
        {
            return SaveMap(progress.Map);
        }

        private MapProgressData LoadMap(string key)
        {
            var defaultData = new MapProgressData(); 
            
            if (string.IsNullOrEmpty(key))
                return defaultData;

            _saveLoader.Load(GetFilePath(key, StorageConstants.MapSubPath), new MapSavedData(), out var data);
            return new MapProgressData
            {
                Key = key,
                Width = data.Width,
                Height = data.Height
            };
        }

        private bool SaveMap(MapProgressData progress)
        {
            var data = new MapSavedData
            {
                Width = progress.Width,
                Height = progress.Height
            };
            return _saveLoader.Save(GetFilePath(progress.Key, StorageConstants.MapSubPath), data);
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