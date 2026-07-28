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

    private void Awake() => Install();

    private void Start()
    {
      using var d = ListPool<IInitializable>.Get(out var initializables);
      Locator.GetAll(initializables);
      foreach (var initializable in initializables)
        initializable.Initialize();

      Locator.GetAll(_tickables);
    }

    private void OnDestroy()
    {
      using var d = ListPool<IDisposable>.Get(out var disposables);
      Locator.GetAll(disposables);
      foreach (var disposable in disposables)
        disposable.Dispose();

      Locator.Clear();
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