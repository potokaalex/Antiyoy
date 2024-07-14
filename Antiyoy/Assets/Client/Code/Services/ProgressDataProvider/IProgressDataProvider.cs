using ClientCode.Data;
using ClientCode.Data.Progress;

namespace ClientCode.Services.ProgressDataProvider
{
    public interface IProgressDataProvider
    {
        ProjectProgressData Project { get; set; }
        public MapEditorProgressData MapEditor { get; set; }
    }
}