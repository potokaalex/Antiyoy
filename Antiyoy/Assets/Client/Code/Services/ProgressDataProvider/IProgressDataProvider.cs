using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Player;
using ClientCode.Data.Progress.Project;

namespace ClientCode.Services.ProgressDataProvider
{
    public interface IProgressDataProvider
    {
        ProjectProgressData Project { get; set; }
        PlayerProgressData Player { get; set; }
    }
}