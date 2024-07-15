using ClientCode.Services.Logger.Base;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;
using ILogHandler = ClientCode.Services.Logger.Base.ILogHandler;

namespace ClientCode.Services.Logger
{
    public class LoggerByPopup : ILogHandler
    {
        private readonly IWindowsHandler _windowsHandler;

        public LoggerByPopup(IWindowsHandler windowsHandler) => _windowsHandler = windowsHandler;

        public void Handle(LogData log)
        {
            var popup = (PopupWindow)_windowsHandler.Get(WindowType.Popup);
            popup.Initialize(log.Message);
        }
    }
}