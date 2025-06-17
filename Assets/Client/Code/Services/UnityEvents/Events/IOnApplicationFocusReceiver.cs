namespace Client.Code.Services.UnityEvents.Events
{
    public interface IOnApplicationFocusReceiver : IUnityEvent
    {
        void OnApplicationFocus(bool hasFocus);
    }
}