using ClientCode.UI.Buttons.Base;

namespace ClientCode.UI.Buttons.Load
{
    public class LoadButton : ButtonBaseOld
    {
        public LoadButtonType Type;

        public override ButtonType GetBaseType() => ButtonType.Load;
    }
}