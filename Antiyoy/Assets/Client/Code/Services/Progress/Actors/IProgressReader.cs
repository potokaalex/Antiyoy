using ClientCode.Data.Progress;

namespace ClientCode.Services.Progress.Actors
{
    public interface IProgressReader : IProgressActor
    {
        void OnLoad(ProgressData progress);
    }
}