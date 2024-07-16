using ClientCode.Services.Logger.Base;
using ClientCode.UI.Windows;
using ClientCode.UI.Windows.Base;
using ClientCode.UI.Windows.Popup;

namespace ClientCode.Services.Logger
{
    public class LoggerByPopup : ILogHandler
    {
        private readonly WindowsFactory _windowsFactory;

        public LoggerByPopup(WindowsFactory windowsFactory) => _windowsFactory = windowsFactory;

        public void Handle(LogData log)
        {
            var popups = (PopupsWindow)_windowsFactory.Get(WindowType.Popups);
            popups.Add(log.Message);
        }
    }
}