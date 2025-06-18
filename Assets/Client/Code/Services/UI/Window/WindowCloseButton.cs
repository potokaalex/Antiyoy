using ClientCode.Infrastructure.States.MapEditor.MainMenu;

namespace Client.Code.Services.UI.Window
{
    public class WindowCloseButton : ButtonView
    {
        public WindowView Window;

        protected override void OnClick() => Window.Close();
    }
}