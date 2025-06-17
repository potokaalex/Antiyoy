using System.Collections.Generic;
using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Data.Saved;
using ClientCode.Data.Static.Const;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClientCode.Services.Progress.Map
{
    public class MapSaveLoader : IMapSaveLoader
    {
        private readonly List<IProgressActor> _actors = new();
        private Task<SaveLoaderResultType> _normalTask;
        private MapProgressData _current;

        public SaveLoaderResultType Load(string key, MapProgressData defaultData = null)
        {
            SaveLoaderDebugger.DebugLoadMap(key);
            if (_current == null || _current.Key != key || string.IsNullOrEmpty(key))
            {
                var result = LoadProgress(key, defaultData, out _current);

                if (result != SaveLoaderResultType.Normal)
                    return result;
            }

            _current.Key = key;

            foreach (var actor in _actors)
                if (actor is IProgressReader<MapProgressData> reader)
                    reader.OnLoad(_current);

            return SaveLoaderResultType.Normal;
        }

        public async UniTask<SaveLoaderResultType> Save(string key = null)
        {
            foreach (var actor in _actors)
                if (actor is IProgressWriter<MapProgressData> writer)
                    await writer.OnSave(_current);

            if (key == null)
                key = _current.Key;
            else
                _current.Key = key;

            SaveLoaderDebugger.DebugSaveMap(key);

            var data = new MapSavedData
            {
                Width = _current.Size.x,
                Height = _current.Size.y,
                Tiles = _current.Tiles,
                Regions = _current.Regions
            };

            if (string.IsNullOrEmpty(key))
                return SaveLoaderResultType.ErrorFileNameIsEmptyOrNull;

            var filePath = ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath);

            return SaveLoader.IsFileExist(filePath)
                ? SaveLoader.Overwrite(filePath, data)
                : SaveLoader.Save(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath), data);
        }

        public SaveLoaderResultType Remove(string key)
        {
            SaveLoaderDebugger.DebugRemoveMap(key);
            return SaveLoader.Remove(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath));
        }

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

        private SaveLoaderResultType LoadProgress(string key, MapProgressData defaultData, out MapProgressData data)
        {
            var result = SaveLoaderResultType.Normal;

            if (string.IsNullOrEmpty(key))
            {
                defaultData ??= new MapProgressData();
                data = defaultData;
            }
            else
            {
                result = SaveLoader.Load(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath), new MapSavedData(),
                    out var savedData);
                data = new MapProgressData
                {
                    Size = new Vector2Int(savedData.Width, savedData.Height),
                    Tiles = savedData.Tiles,
                    Regions = savedData.Regions
                };
            }

            return result;
        }
    }
}