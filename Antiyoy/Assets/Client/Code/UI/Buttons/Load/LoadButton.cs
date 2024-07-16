using ClientCode.UI.Buttons.Base;

namespace ClientCode.UI.Buttons.Load
{
    public class LoadButton : ButtonBase
    {
        public LoadButtonType Type;

        public override ButtonType GetBaseType() => ButtonType.Load;
    }
}