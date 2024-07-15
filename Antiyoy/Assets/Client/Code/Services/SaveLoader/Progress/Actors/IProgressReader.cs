using ClientCode.Data.Progress;

namespace ClientCode.Services.SaveLoader.Progress.Actors
{
    public interface IProgressReader : IProgressActor
    {
        void OnLoad(PlayerProgressData progress);
    }
}