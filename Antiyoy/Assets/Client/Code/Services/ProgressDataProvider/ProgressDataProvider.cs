using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;

namespace ClientCode.Services.ProgressDataProvider
{
    public class ProgressDataProvider : IProgressDataProvider
    {
        public ProjectProgressData Project { get; set; }

        public PlayerProgressData Player { get; set; }
    }
}