using ClientCode.Data.Progress;

namespace ClientCode.Services.ProgressDataProvider
{
    public interface IProgressDataProvider
    {
        MapEditorProgressData MapEditor { get; set; }
        MainMenuProgressData MainMenu { get; set; }
    }
}