using ClientCode.Services.Progress.Actors;

namespace ClientCode.Services.Progress.Base
{
    public interface IProgressSaveLoader
    {
        void RegisterActor(IProgressActor actor);
        void UnRegisterActor(IProgressActor actor);
    }
}