using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;

namespace ClientCode.Services.ProgressDataProvider
{
    public interface IProgressDataProvider
    {
        ProjectProgressData Project { get; set; }
        MapEditorProgressData MapEditor { get; set; }
        MainMenuProgressData MainMenu { get; set; }
    }
}