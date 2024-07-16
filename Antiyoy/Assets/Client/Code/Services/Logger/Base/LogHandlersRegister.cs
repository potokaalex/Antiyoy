using System;
using System.Collections.Generic;
using Zenject;

namespace ClientCode.Services.Logger.Base
{
    public class LogHandlersRegister : IInitializable, IDisposable
    {
        private readonly ILogReceiver _receiver;
        private readonly List<ILogHandler> _handlers;

        public LogHandlersRegister(ILogReceiver receiver, List<ILogHandler> handlers)
        {
            _receiver = receiver;
            _handlers = handlers;
        }

        public void Initialize()
        {
            foreach (var handler in _handlers)
                _receiver.RegisterHandler(handler);
        }

        public void Dispose()
        {
            foreach (var handler in _handlers)
                _receiver.UnRegisterHandler(handler);
        }
    }
}