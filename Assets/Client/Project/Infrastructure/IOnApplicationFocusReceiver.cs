namespace Client.Project.Infrastructure
{
  public interface IOnApplicationFocusReceiver
  {
    void OnFocus(bool hasFocus);
  }
}