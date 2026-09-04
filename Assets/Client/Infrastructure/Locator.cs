using System;
using System.Collections.Generic;

namespace Client.Infrastructure
{
  public static class Locator
  {
    private static readonly Dictionary<Type, List<object>> _services = new();

    public static void Add<T>(Type contract, T service) => GetListOrCreate(contract).Add(service);

    public static T Get<T>() => (T)_services[typeof(T)][0];

    private static List<object> GetListOrCreate(Type contract)
    {
      if (!_services.TryGetValue(contract, out var list))
      {
        list = new List<object>();
        _services[contract] = list;
      }

      return list;
    }

    public static void Clear() => _services.Clear();
  }
}