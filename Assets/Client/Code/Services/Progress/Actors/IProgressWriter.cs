namespace Client.Code.Services.Progress.Actors
{
    public interface IProgressWriter : IProgressActor
    {
        void OnWrite(ProgressData progress);
    }
}