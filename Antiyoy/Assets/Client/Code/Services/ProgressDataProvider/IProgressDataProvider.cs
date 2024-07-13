using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Load;

namespace ClientCode.Services.ProgressDataProvider
{
    public interface IProgressDataProvider
    {
        ProjectProgressData Project { get; set; }
        MapProgressData Map { get; set; }
        ILoadData Load { get; set; }
    }
}