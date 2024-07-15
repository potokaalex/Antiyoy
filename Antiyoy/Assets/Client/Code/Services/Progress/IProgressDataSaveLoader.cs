using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.Progress.Actors;

namespace ClientCode.Services.Progress
{
    public interface IProgressDataSaveLoader
    {
        ProjectProgressData LoadProject();
        void LoadPlayer();
        Task SavePlayer();
        MapProgressData LoadMap(string key);
        void RegisterActor(IProgressActor actor);
        void UnRegisterActor(IProgressActor actor);
    }
}