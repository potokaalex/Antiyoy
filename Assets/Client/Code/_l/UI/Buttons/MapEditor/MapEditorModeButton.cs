using ClientCode.UI.Buttons.Base;

namespace ClientCode.UI.Buttons.MapEditor
{
    public class MapEditorModeButton : ButtonBaseOld
    {
        public MapEditorModeButtonType Type;

        public override ButtonType GetBaseType() => ButtonType.MapEditorMode;
    }
}