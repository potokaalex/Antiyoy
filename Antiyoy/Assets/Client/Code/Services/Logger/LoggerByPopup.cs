using ClientCode.Services.Logger.Base;
using ClientCode.UI.Windows.Base;
using ClientCode.UI.Windows.Popup;
using ILogHandler = ClientCode.Services.Logger.Base.ILogHandler;

namespace ClientCode.Services.Logger
{
    public class LoggerByPopup : ILogHandler
    {
        private readonly IWindowsHandler _windowsHandler;

        public LoggerByPopup(IWindowsHandler windowsHandler) => _windowsHandler = windowsHandler;

        public void Handle(LogData log)
        {
            var popups = (PopupsWindow)_windowsHandler.Get(WindowType.Popups);
            popups.Add(log.Message);
        }
    }
}