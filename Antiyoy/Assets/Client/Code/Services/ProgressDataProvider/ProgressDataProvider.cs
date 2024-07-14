using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;

namespace ClientCode.Services.ProgressDataProvider
{
    public class ProgressDataProvider : IProgressDataProvider
    {
        public ProjectProgressData Project { get; set; }

        public MapEditorProgressData MapEditor { get; set; }

        public MainMenuProgressData MainMenu { get; set; }
    }
}