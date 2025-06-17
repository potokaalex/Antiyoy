using System.Collections.Generic;
using Client.Code.Services.UnityEvents.Events;
using UnityEngine;

namespace Client.Code.Services.UnityEvents
{
    public class UnityEventsSender : MonoBehaviour
    {
        private readonly List<IOnApplicationFocusReceiver> _applicationFocusReceivers = new();

        public void Register(IUnityEvent unityEvent)
        {
            if (unityEvent is IOnApplicationFocusReceiver onApplicationFocusReceiver)
                _applicationFocusReceivers.Add(onApplicationFocusReceiver);
        }

        public void UnRegister(IUnityEvent unityEvent)
        {
            if (unityEvent is IOnApplicationFocusReceiver onApplicationFocusReceiver)
                _applicationFocusReceivers.Remove(onApplicationFocusReceiver);
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            for (var i = 0; i < _applicationFocusReceivers.Count; i++)
                _applicationFocusReceivers[i].OnApplicationFocus(hasFocus);
        }
    }
}