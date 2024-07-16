using ClientCode.UI.Buttons.Base;

namespace ClientCode.UI.Buttons.Map.SaveLoad
{
    public class MapSaveLoadButton : ButtonBase
    {
        public MapSaveLoadButtonType Type;

        public override ButtonType GetBaseType() => ButtonType.MapSaveLoad;
    }
}