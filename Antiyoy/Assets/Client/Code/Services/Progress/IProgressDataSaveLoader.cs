using System.Threading.Tasks;
using ClientCode.Data.Progress.Player;
using ClientCode.Services.Progress.Actors;

namespace ClientCode.Services.Progress
{
    public interface IProgressDataSaveLoader
    {
        void Load();
        Task Save();
        MapProgressData LoadMap(string key);
        void RegisterActor(IProgressActor actor);
        void UnRegisterActor(IProgressActor actor);
    }
}