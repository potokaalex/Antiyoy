using System.Threading.Tasks;
using ClientCode.Data.Progress.Player;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress
{
    public interface IProgressDataSaveLoader
    {
        void Load();
        Task Save();
        SaveLoaderResultType IsValidMapKeyToSaveWithoutOverwrite(string key);
        MapProgressData LoadMap(string key);
        void RegisterActor(IProgressActor actor);
        void UnRegisterActor(IProgressActor actor);
    }
}