using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Load;

namespace ClientCode.Services.SaveLoader.Progress
{
    public interface IProgressDataSaveLoader
    {
        ProjectLoadData LoadProjectLoadData();
        ProjectProgressData LoadProjectProgress(ProjectProgressData defaultData);
        MapProgressData LoadMapProgress(string key, MapProgressData defaultData);
    }
}