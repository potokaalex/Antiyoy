using ClientCode.UI.Buttons.Base;

namespace ClientCode.UI.Buttons.MapEditor
{
    public class MapEditorModeButton : ButtonBase
    {
        public MapEditorModeButtonType Type;

        public override ButtonType GetBaseType() => ButtonType.MapEditorMode;
    }
}