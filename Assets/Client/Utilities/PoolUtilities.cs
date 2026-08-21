using UnityEngine.Pool;

namespace Client.Utilities
{
  public static class PoolUtilities
  {
    public static void Prewarm<T>(this ObjectPool<T> pool, int count) where T : class
    {
      using (ListPool<T>.Get(out var items))
      {
        for (var i = 0; i < count; i++) 
          items.Add(pool.Get());

        for (var i = 0; i < count; i++) 
          pool.Release(items[i]);
      }
    } 
  }
}