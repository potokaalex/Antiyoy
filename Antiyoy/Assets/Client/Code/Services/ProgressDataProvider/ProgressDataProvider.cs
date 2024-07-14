using ClientCode.Data;
using ClientCode.Data.Progress;

namespace ClientCode.Services.ProgressDataProvider
{
    public class ProgressDataProvider : IProgressDataProvider
    {
        public MapEditorProgressData MapEditor { get; set; }
        
        public MainMenuProgressData MainMenu { get; set; }
    }
}