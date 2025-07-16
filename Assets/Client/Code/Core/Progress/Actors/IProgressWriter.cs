namespace Client.Code.Core.Progress.Actors
{
    public interface IProgressWriter : IProgressActor
    {
        void OnWrite(ProgressData progress);
    }
}