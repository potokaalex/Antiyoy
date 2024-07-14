namespace ClientCode.UI.Windows.Base
{
    public interface IWindowsHandler
    {
        WindowBase Get(WindowType type);
        void OnBeforeOpen(WindowBase window);
    }
}