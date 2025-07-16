namespace Client.Code.Core.UnityEvents.Events
{
    public interface IOnApplicationFocusReceiver : IUnityEvent
    {
        void OnApplicationFocus(bool hasFocus);
    }
}