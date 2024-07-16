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
        SaveLoaderResultType IsMapKeyValidToSaveWithoutOverwrite(string key);
        SaveLoaderResultType IsMapValidToLoad(string key);
        MapProgressData LoadMap(string key);
        SaveLoaderResultType RemoveMap(string key);
        void RegisterActor(IProgressActor actor);
        void UnRegisterActor(IProgressActor actor);
    }
}