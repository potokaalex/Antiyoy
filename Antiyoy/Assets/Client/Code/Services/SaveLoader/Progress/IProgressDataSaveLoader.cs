using ClientCode.Data;
using ClientCode.Data.Progress;

namespace ClientCode.Services.SaveLoader.Progress
{
    public interface IProgressDataSaveLoader
    {
        ProjectProgressData LoadProject();
        MainMenuProgressData LoadMainMenu();
        MapEditorProgressData LoadMapEditor(string mapKey);
    }
}