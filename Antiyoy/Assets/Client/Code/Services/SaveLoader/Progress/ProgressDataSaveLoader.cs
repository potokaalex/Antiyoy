using System.Collections.Generic;
using System.IO;
using ClientCode.Data;
using ClientCode.Data.Const;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.SaveLoader.Base;
using ClientCode.Services.SaveLoader.Progress.Actors;

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

        private PlayerProgressData _player;

        public PlayerProgressData LoadPlayer()
        {
            if (_player == null)
            {
                _player = new PlayerProgressData
                {
                    MapKeys = _saveLoader.GetFileNames(GetPath(StorageConstants.MapSubPath), StorageConstants.FilesExtension)
                };
            }

            foreach (var actor in _actors)
            {
                if (actor is IProgressReader reader)
                    reader.OnLoad(_player);
            }

            return _player;
        }

        public bool SavePlayer()
        {
            foreach (var actor in _actors)
            {
                if (actor is IProgressWriter writer)
                    writer.OnSave(_player);
            }
            
            return SaveMap(_player.Map);
        }

        private readonly List<IProgressActor> _actors = new();

        public void RegisterActor(IProgressActor actor) => _actors.Add(actor);

        public void UnRegisterActor(IProgressActor actor) => _actors.Remove(actor);

        public MapProgressData LoadMap(string key)
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