using System;
using System.Collections.Generic;
using Client.Code.Core.UnityEvents.Events;
using Zenject;

namespace Client.Code.Core.UnityEvents
{
    public class UnityEventsRegister : IInitializable, IDisposable
    {
        private readonly UnityEventsSender _eventsSender;
        private readonly List<IUnityEvent> _events;

        public UnityEventsRegister(UnityEventsSender eventsSender, List<IUnityEvent> events)
        {
            _eventsSender = eventsSender;
            _events = events;
        }

        public void Initialize()
        {
            for (var i = 0; i < _events.Count; i++)
                _eventsSender.Register(_events[i]);
        }

        public void Dispose()
        {
            for (var i = 0; i < _events.Count; i++)
                _eventsSender.UnRegister(_events[i]);
        }
    }
}