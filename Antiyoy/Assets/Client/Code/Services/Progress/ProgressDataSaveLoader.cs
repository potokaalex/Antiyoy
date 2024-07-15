using System.Collections.Generic;
using System.Threading.Tasks;
using ClientCode.Data;
using ClientCode.Data.Const;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.SaveLoad;

namespace ClientCode.Services.Progress
{
    public class ProgressDataSaveLoader : IProgressDataSaveLoader
    {
        private readonly ProjectLoadData _projectLoadData;
        private PlayerProgressData _player;

        public ProgressDataSaveLoader(ProjectLoadData projectLoadData)
        {
            _projectLoadData = projectLoadData;
        }

        public ProjectProgressData LoadProject()
        {
            return new ProjectProgressData
            {
                Load = _projectLoadData
            };
        }

        public void LoadPlayer()
        {
            _player ??= new PlayerProgressData();
            _player.MapKeys = SaveLoader.GetFileNames(ProgressPathTool.GetPath(StorageConstants.MapSubPath), StorageConstants.FilesExtension);
            
            foreach (var actor in _actors)
            {
                if (actor is IProgressReader reader)
                    reader.OnLoad(_player);
            }
        }

        public async Task SavePlayer()
        {
            foreach (var actor in _actors)
            {
                if (actor is IProgressWriter writer)
                    await writer.OnSave(_player);
            }

            SaveMap(_player.Map);
        }

        private readonly List<IProgressActor> _actors = new();

        public void RegisterActor(IProgressActor actor) => _actors.Add(actor);

        public void UnRegisterActor(IProgressActor actor) => _actors.Remove(actor);

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