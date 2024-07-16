using System.Collections.Generic;
using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Player;
using ClientCode.Data.Progress.Project;
using ClientCode.Data.Saved;
using ClientCode.Data.Static.Const;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress
{
    public class ProgressDataSaveLoader : IProgressDataSaveLoader
    {
        private readonly ProjectLoadData _projectLoadData;
        private readonly ILogReceiver _logReceiver;
        private readonly List<IProgressActor> _actors = new();
        private ProgressData _progress;

        public ProgressDataSaveLoader(ProjectLoadData projectLoadData, ILogReceiver logReceiver)
        {
            _projectLoadData = projectLoadData;
            _logReceiver = logReceiver;
        }

        public void RegisterActor(IProgressActor actor) => _actors.Add(actor);

        public void UnRegisterActor(IProgressActor actor) => _actors.Remove(actor);

        public void Load()
        {
            _progress ??= new ProgressData { Project = { Load = _projectLoadData } };

            _progress.Player.MapKeys =
                SaveLoader.GetFileNames(ProgressPathTool.GetPath(StorageConstants.MapSubPath), StorageConstants.FilesExtension);

            foreach (var actor in _actors)
            {
                if (actor is IProgressReader reader)
                    reader.OnLoad(_progress);
            }
        }

        public async Task Save()
        {
            foreach (var actor in _actors)
            {
                if (actor is IProgressWriter writer)
                    await writer.OnSave(_progress);
            }

            SaveMap(_progress.Player.Map);
        }

        public SaveLoaderResultType IsMapKeyValidToSaveWithoutOverwrite(string key)
        {
            var path = ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath);
            return SaveLoader.IsValidSavePath(path);
        }

        public SaveLoaderResultType IsMapValidToLoad(string key)
        {
            var result = SaveLoader.Load<MapSavedData>(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath), null, out _);
            return result;
        }

        public MapProgressData LoadMap(string key)
        {
            var defaultData = new MapProgressData();

            if (string.IsNullOrEmpty(key))
                return defaultData;

            var result = SaveLoader.Load(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath), new MapSavedData(), out var data);

            if (result == SaveLoaderResultType.ErrorFileIsNotExist)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: file is not exits!"));
            else if (result == SaveLoaderResultType.ErrorFileIsDamaged)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: file is damaged!"));
            else if (result == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: unknown reason!"));

            return new MapProgressData
            {
                Key = key,
                Width = data.Width,
                Height = data.Height
            };
        }

        private void SaveMap(MapProgressData progress)
        {
            var data = new MapSavedData
            {
                Width = progress.Width,
                Height = progress.Height
            };

            var filePath = ProgressPathTool.GetFilePath(progress.Key, StorageConstants.MapSubPath);

            if (SaveLoader.IsFileExist(filePath))
            {
                if (SaveLoader.Overwrite(filePath, data) == SaveLoaderResultType.Error)
                    _logReceiver.Log(new LogData(LogType.Error, "Impossible to overwrite: unknown reason!"));
                return;
            }

            var result = SaveLoader.Save(ProgressPathTool.GetFilePath(progress.Key, StorageConstants.MapSubPath), data);
            if (result == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to save: unknown reason!"));
        }
    }
}