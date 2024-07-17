using System.Threading.Tasks;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress.Project
{
    public interface IProjectSaveLoader : IProgressSaveLoader<ProjectProgressData>
    {
        ProjectProgressData Current { get; }
        Task Load();
        Task Save();
    }
}