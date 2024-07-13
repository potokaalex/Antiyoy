using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Load;

namespace ClientCode.Services.ProgressDataProvider
{
    public class ProgressDataProvider : IProgressDataProvider
    {
        public ProgressDataProvider(ILoadData loadData) => Load = loadData;

        public ProjectProgressData Project { get; set; }

        public MapProgressData Map { get; set; }

        public ILoadData Load { get; set; }
    }
}