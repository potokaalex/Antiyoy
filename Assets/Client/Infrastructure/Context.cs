using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Infrastructure
{
  [DefaultExecutionOrder(-10000)]
  public class Context : MonoBehaviour
  {
    [SerializeField] private MonoInstaller[] _installers;
    private readonly List<ITickable> _tickables = new();
    private readonly List<Type> _registrations = new();

    public void Register<T>(T service) => Register(typeof(T), service);

    public void Register<T>(Type contract, T service)
    {
      _registrations.Add(contract);
      Locator.Set(contract, service);
    }

    private void Awake()
    {
      foreach (var installer in _installers) 
        installer.Install(this);
    }

    private void Start()
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
  }
}