using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.SaveLoader.Progress.Actors;

namespace ClientCode.Services.SaveLoader.Progress
{
    public interface IProgressDataSaveLoader
    {
        ProjectProgressData LoadProject();
        PlayerProgressData LoadPlayer();
        bool SavePlayer();
        MapProgressData LoadMap(string key);
        void RegisterActor(IProgressActor actor);
        void UnRegisterActor(IProgressActor actor);
    }
}