using System.Collections.Generic;
using UnityEngine.Pool;

namespace Client.Utilities
{
  public class QueuePool<T>
  {
    private static readonly ObjectPool<Queue<T>> _pool = new(() => new Queue<T>(), actionOnRelease: x => x.Clear());

    public static PooledObject<Queue<T>> Get(out Queue<T> value)
    {
      return _pool.Get(out value);
    }

    public static void Release(Queue<T> toRelease)
    {
      _pool.Release(toRelease);
    }
  }
}