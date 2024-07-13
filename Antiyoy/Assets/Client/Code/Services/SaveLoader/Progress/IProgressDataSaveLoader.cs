using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Load;

namespace ClientCode.Services.SaveLoader.Progress
{
    public interface IProgressDataSaveLoader
    {
        ProjectLoadData LoadProjectLoadData();
        bool LoadProjectProgress(ProjectProgressData defaultData);
        void LoadMapProgress(string key, MapProgressData defaultData);
    }
}