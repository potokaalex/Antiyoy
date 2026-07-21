namespace Client.Utilities
{
  public static class PlatformUtilities
  {
    public static bool IsEditor { get; }

    static PlatformUtilities()
    {
#if UNITY_EDITOR
      IsEditor = true;
#else
      IsEditor = false;
#endif
    }
  }
}