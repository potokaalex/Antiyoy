using System.Collections.Generic;
using ClientCode.Data.Progress.Project;
using ClientCode.Data.Static.Const;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress.Project
{
    public class ProjectSaveLoader : IProjectSaveLoader
    {
        private readonly ProjectLoadData _projectLoadData;
        private readonly List<IProgressActor> _actors = new();
        private ProjectProgressData _currentProgress;

        public ProjectSaveLoader(ProjectLoadData projectLoadData) => _projectLoadData = projectLoadData;

        public void Load(out ProjectProgressData progress)
        {
            Load();
            progress = _currentProgress;
        }

        public void Load()
        {
            SaveLoaderDebugger.DebugLoadProject();
            _currentProgress ??= new ProjectProgressData { Load = _projectLoadData };

            var fileNames = SaveLoader.GetFileNames(ProgressPathTool.GetPath(StorageConstants.MapSubPath), StorageConstants.FilesExtension);
            _currentProgress.MapKeys = fileNames;

            foreach (var actor in _actors)
                if (actor is IProgressReader<ProjectProgressData> reader)
                    reader.OnLoad(_currentProgress);
        }

        public async void Save()
        {
            SaveLoaderDebugger.DebugSaveProject();
            foreach (var actor in _actors)
                if (actor is IProgressWriter<ProjectProgressData> writer)
                    await writer.OnSave(_currentProgress);
        }

        public void RegisterActor(IProgressActor actor) => _actors.Add(actor);

        public void UnRegisterActor(IProgressActor actor) => _actors.Remove(actor);
    }
}