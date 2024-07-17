using System.Collections.Generic;
using System.Threading.Tasks;
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

        public ProjectSaveLoader(ProjectLoadData projectLoadData) => _projectLoadData = projectLoadData;

        public ProjectProgressData Current { get; private set; }

        public async Task Load()
        {
            Current ??= new ProjectProgressData { Load = _projectLoadData };

            var fileNames = SaveLoader.GetFileNames(ProgressPathTool.GetPath(StorageConstants.MapSubPath), StorageConstants.FilesExtension);
            Current.MapKeys = fileNames;

            foreach (var actor in _actors)
                if (actor is IProgressReader<ProjectProgressData> reader)
                    await reader.OnLoad(Current);
        }

        public async Task Save()
        {
            foreach (var actor in _actors)
                if (actor is IProgressWriter<ProjectProgressData> writer)
                    await writer.OnSave(Current);
        }

        public void RegisterActor(IProgressActor actor) => _actors.Add(actor);

        public void UnRegisterActor(IProgressActor actor) => _actors.Remove(actor);
    }
}