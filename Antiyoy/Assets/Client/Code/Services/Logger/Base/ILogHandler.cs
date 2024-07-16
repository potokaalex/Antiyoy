namespace ClientCode.Services.Logger.Base
{
    public interface ILogHandler
    {
        void Handle(LogData log);
    }
}