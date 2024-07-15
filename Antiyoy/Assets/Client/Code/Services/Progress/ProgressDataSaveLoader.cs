using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Player;
using ClientCode.Data.Progress.Project;
using ClientCode.Data.Saved;
using ClientCode.Data.Static.Const;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress
{
    public class ProgressDataSaveLoader : IProgressDataSaveLoader
    {
        private readonly ProjectLoadData _projectLoadData;
        private readonly List<IProgressActor> _actors = new();
        private ProgressData _progress;

        public ProgressDataSaveLoader(ProjectLoadData projectLoadData) => _projectLoadData = projectLoadData;
        
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

        public MapProgressData LoadMap(string key)
        {
            var defaultData = new MapProgressData();

            if (string.IsNullOrEmpty(key))
                return defaultData;

            SaveLoader.Load(ProgressPathTool.GetFilePath(key, StorageConstants.MapSubPath), new MapSavedData(), out var data);
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
            SaveLoader.Save(ProgressPathTool.GetFilePath(progress.Key, StorageConstants.MapSubPath), data);
        }
    }
}