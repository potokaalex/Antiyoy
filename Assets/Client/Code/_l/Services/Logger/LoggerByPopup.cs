using ClientCode.Services.Logger.Base;

namespace ClientCode.Services.Logger
{
    public class LoggerByPopup : ILogHandler
    {
        public void Handle(LogData log)
        {
            //var popups = (PopupsWindow)_windowsFactory.Get(WindowType.Popups);
            //popups.Add(log.Message);
        }
    }
}