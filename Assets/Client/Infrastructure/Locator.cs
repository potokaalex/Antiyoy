using System;
using System.Collections.Generic;

namespace Client.Infrastructure
{
  public static class Locator
  {
    private static readonly Dictionary<Type, object> _services = new();

    public static void Set<T>(T service) => _services[typeof(T)] = service;

    public static void Remove(Type contract) => _services.Remove(contract);

    public static T Get<T>() => (T)_services[typeof(T)];

    public static void GetAll<T>(List<T> outList, List<Type> contracts)
    {
      outList.Clear();
      var findType = typeof(T);

      foreach (var contract in contracts)
      {
        if (findType.IsAssignableFrom(contract))
          if (_services.TryGetValue(contract, out var service))
            outList.Add((T)service);
      }
    }
  }
}