using UniRx;
using UnityEngine;

namespace Client.Code.Services.UI.Window
{
    public class WindowView : MonoBehaviour
    {
        public bool ControlActive;
        
        public bool IsOpen { get; private set; } = true;

        public Subject<Unit> OnOpenEvent { get; } = new();

        public Subject<Unit> OnCloseEvent { get; } = new();

        public void Open(bool force = false)
        {
            if (!IsOpen)
            {
                IsOpen = true;
                OnOpen(force);
                if(ControlActive)
                    gameObject.SetActive(true);
                OnOpenEvent.OnNext(default);
            }
        }

        public void Close(bool force = false)
        {
            if (IsOpen)
            {
                IsOpen = false;
                OnClose(force);
                if(ControlActive)
                    gameObject.SetActive(false);
                OnCloseEvent.OnNext(default);
            }
        }

        protected virtual void OnOpen(bool force)
        {
        }

        protected virtual void OnClose(bool force)
        {
        }
    }
}