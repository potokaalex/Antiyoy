using System;
using System.Collections.Generic;

namespace Client.Infrastructure
{
  public static class Locator
  {
    private static readonly Dictionary<Type, object> _services = new();

    public static void Set<T>(Type contract, T service) => _services.Add(contract, service);

    public static T Get<T>() => (T)_services[typeof(T)];

    public static void Clear() => _services.Clear();
  }
}