using System.Collections.Generic;
using UnityEngine.Pool;

namespace Client.Utilities
{
  public class StackPool<T>
  {
    private static readonly ObjectPool<Stack<T>> _pool = new(() => new Stack<T>(), actionOnRelease: x => x.Clear());

    public static PooledObject<Stack<T>> Get(out Stack<T> value) => _pool.Get(out value);

    public static void Release(Stack<T> toRelease) => _pool.Release(toRelease);
  }
}