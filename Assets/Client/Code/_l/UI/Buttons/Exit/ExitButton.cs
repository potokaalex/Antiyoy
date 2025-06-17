using ClientCode.UI.Buttons.Base;

namespace ClientCode.UI.Buttons.Exit
{
    public class ExitButton : ButtonBase
    {
        public ExitButtonType Type;
        
        public override ButtonType GetBaseType() => ButtonType.Exit;
    }
}