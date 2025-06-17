using ClientCode.Data.Progress.Project;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress.Project
{
    public interface IProjectSaveLoader : IProgressSaveLoader
    {
        void Load(out ProjectProgressData progress);
        void Load();
        void Save();
    }
}