using ClientCode.Data.Scene;
using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Handlers
{
    public class MapEditorWindowsHandler : WindowsHandlerBase
    {
        public MapEditorWindowsHandler(UIFactory factory, MapEditorSceneData sceneData) : base(factory, sceneData.UIRoot)
        {
        }
    }
}