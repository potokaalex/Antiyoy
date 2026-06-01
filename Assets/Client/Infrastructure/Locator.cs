using System;
using System.Collections.Generic;

namespace Client.Infrastructure
{
  public static class Locator
  {
    private static readonly Dictionary<Type, object> _items = new();

    public static void Set<T>(T item) => _items[typeof(T)] = item;

    public static T Get<T>() => (T)_items[typeof(T)];

    public static void GetAll<T>(List<T> outList)
    {
      outList.Clear();
      var findType = typeof(T);
      foreach ((var type, var item) in _items)
        if (findType.IsAssignableFrom(type))
          outList.Add((T)item);
    }

    public static void Clear() => _items.Clear();
  }
}