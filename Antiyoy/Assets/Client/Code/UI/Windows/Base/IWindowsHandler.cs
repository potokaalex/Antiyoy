namespace ClientCode.UI.Windows.Base
{
    public interface IWindowsHandler
    {
        WindowBase Get(WindowType windowType);
        void OnBeforeOpen(WindowBase window);
        void OnAfterClose(WindowBase window);
    }
}