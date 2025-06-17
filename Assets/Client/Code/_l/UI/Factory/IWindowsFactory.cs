using ClientCode.UI.Windows.Base;

namespace ClientCode.UI.Factory
{
    public interface IWindowsFactory
    {
        IWindow Get(WindowType type, bool isNew = false);
    }
}