using ClientCode.Data.Progress;

namespace ClientCode.Services.SaveLoader.Progress.Actors
{
    public interface IProgressWriter : IProgressActor
    {
        void OnSave(PlayerProgressData progress);
    }
}