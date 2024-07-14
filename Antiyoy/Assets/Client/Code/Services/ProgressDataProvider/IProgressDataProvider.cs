using ClientCode.Data;
using ClientCode.Data.Progress;

namespace ClientCode.Services.ProgressDataProvider
{
    public interface IProgressDataProvider //TODO: обобщить ?
    {
        MapEditorProgressData MapEditor { get; set; }
        MainMenuProgressData MainMenu { get; set; }
    }
}