using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Infrastructure
{
  [DefaultExecutionOrder(-10000)]
  public abstract class MonoInstaller : MonoBehaviour
  {
    private readonly List<ITickable> _tickables = new();
    private readonly List<Type> _registrations = new();

    private protected void Register<T>(T service)
    {
      _registrations.Add(typeof(T));
      Locator.Set(service);
    }

    private void Awake() => Install();

    protected virtual void Start()
    {
      using (ListPool<IInitializable>.Get(out var initializables))
      {
        Locator.GetAll(initializables, _registrations);
        foreach (var initializable in initializables)
          initializable.Initialize();
      }

      Locator.GetAll(_tickables, _registrations);
    }

    private void OnDestroy()
    {
      using (ListPool<IDisposable>.Get(out var disposables))
      {
        Locator.GetAll(disposables, _registrations);
        foreach (var disposable in disposables)
          disposable.Dispose();

        foreach (var registration in _registrations)
          Locator.Remove(registration);
      }
    }

    private void Update()
    {
      foreach (var tickable in _tickables)
        tickable.Tick();
    }

    protected virtual void Install()
    {
    }
  }
}