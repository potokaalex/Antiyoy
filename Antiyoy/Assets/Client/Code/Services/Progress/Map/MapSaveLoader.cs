using System.Collections.Generic;
using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Data.Saved;
using ClientCode.Data.Static.Const;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress.Map
{
    public class MapSaveLoader : IMapSaveLoader
    {
        private readonly List<IProgressActor> _actors = new();
        private Task<SaveLoaderResultType> _normalTask;
        private MapProgressData _progress;

        public string CurrentKey => _progress.Key;

        public async Task<SaveLoaderResultType> Load(string key)
        {
            if (_progress == null || key != _progress.Key)
            {
                var result = LoadProgress(key, out _progress);

                if (result != SaveLoaderResultType.Normal)
                    return result;
            }

            foreach (var actor in _actors)
                if (actor is IProgressReader<MapProgressData> reader)
                    await reader.OnLoad(_progress);

            return await CreateNormalTask();
        }

        public async Task<SaveLoaderResultType> Save()
        {
            foreach (var actor in _actors)
                if (actor is IProgressWriter<MapProgressData> reader)
                    await reader.OnSave(_progress);

            var data = new MapSavedData
            {
                Width = _progress.Width,
                Height = _progress.Height,
                Tiles = _progress.Tiles,
                Regions = _progress.Regions
            };

            var filePath = ProgressPathTool.GetFilePath(_progress.Key, StorageConstants.MapSubPath);

            return SaveLoader.IsFileExist(filePath)
                ? SaveLoader.Overwrite(filePath, data)
                : SaveLoader.Save(ProgressPathTool.GetFilePath(_progress.Key, StorageConstants.MapSubPath), data);
        }

        public SaveLoaderResultType Remove(string key) => SaveLoader.Remove(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath));

        public SaveLoaderResultType IsKeyValidToSaveWithoutOverwrite(string key)
        {
            var path = ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath);
            return string.IsNullOrEmpty(key) ? SaveLoaderResultType.ErrorFileNameIsEmptyOrNull : SaveLoader.IsValidSavePath(path);
        }

        public SaveLoaderResultType IsValidToLoad(string key)
        {
            var result = SaveLoader.Load<MapSavedData>(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath), null, out _);
            return result;
        }

        public void RegisterActor(IProgressActor actor) => _actors.Add(actor);

        public void UnRegisterActor(IProgressActor actor) => _actors.Remove(actor);

        private SaveLoaderResultType LoadProgress(string key, out MapProgressData data)
        {
            var result = SaveLoaderResultType.Normal;

            if (string.IsNullOrEmpty(key))
                data = new MapProgressData();
            else
            {
                result = SaveLoader.Load(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath), new MapSavedData(),
                    out var savedData);
                data = new MapProgressData
                {
                    Key = key,
                    Width = savedData.Width,
                    Height = savedData.Height,
                    Tiles = savedData.Tiles,
                    Regions = savedData.Regions
                };
            }

            return result;
        }

        private Task<SaveLoaderResultType> CreateNormalTask() => _normalTask ??= Task.FromResult(SaveLoaderResultType.Normal);
    }
}