using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;

namespace ClientCode.Services.SaveLoader.Progress
{
    public interface IProgressDataSaveLoader
    {
        ProjectProgressData LoadProject();
        MainMenuProgressData LoadMainMenu();
        MapEditorProgressData LoadMapEditor(string mapKey);
        bool SaveMapEditor(MapProgressData progress);
    }
}