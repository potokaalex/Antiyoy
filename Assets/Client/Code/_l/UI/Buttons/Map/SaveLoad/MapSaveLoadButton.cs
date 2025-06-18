using ClientCode.UI.Buttons.Base;

namespace ClientCode.UI.Buttons.Map.SaveLoad
{
    public class MapSaveLoadButton : ButtonBaseOld
    {
        public MapSaveLoadButtonType Type;

        public override ButtonType GetBaseType() => ButtonType.MapSaveLoad;
    }
}