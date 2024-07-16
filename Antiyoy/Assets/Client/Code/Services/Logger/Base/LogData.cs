namespace ClientCode.Services.Logger.Base
{
    public struct LogData
    {
        public LogType Type;
        public string Message;

        public LogData(LogType type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}