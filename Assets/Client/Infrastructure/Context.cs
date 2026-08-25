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

    public void Register(object service, params Type[] contracts)
    {
      var serviceType = service.GetType();

      foreach (var contract in contracts)
      {
        if (!contract.IsAssignableFrom(serviceType))
          Debug.LogError($"{serviceType} is not implementing {contract}");

        Locator.Add(contract, service);
      }
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
        Locator.GetAll(initializables);
        foreach (var initializable in initializables)
          initializable.Initialize();
      }

      Locator.GetAll(_tickables);
    }

    private void Update()
    {
      foreach (var tickable in _tickables)
        tickable.Tick();
    }
  }
}