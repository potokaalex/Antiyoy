using ClientCode.Data;
using ClientCode.Data.Progress;

namespace ClientCode.Services.ProgressDataProvider
{
    public class ProgressDataProvider : IProgressDataProvider
    {
        public ProjectProgressData Project { get; set; }
        
        public MapEditorProgressData MapEditor { get; set; }
    }
}