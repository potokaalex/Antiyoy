using ClientCode.Services.Progress.Actors;

namespace ClientCode.Services.Progress.Base
{
    public interface IProgressSaveLoader<T> where T : IProgressData
    {
        void RegisterActor(IProgressActor actor);
        void UnRegisterActor(IProgressActor actor);
    }
}