namespace ClientCode.UI.Windows.Base
{
    public interface IWindowsHandler
    {
        WindowBase Get(WindowType type, bool isNew = false);
        void OnBeforeOpen(WindowBase window);
    }
}