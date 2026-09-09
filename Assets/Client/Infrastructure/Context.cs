using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Infrastructure
{
  [DefaultExecutionOrder(-10000)]
  public class Context : MonoBehaviour
  {
    [SerializeField] private MonoInstaller[] _installers;
    private readonly List<IInitializable> _initializables = new();
    private readonly List<ITickable> _tickables = new();

    public void Register(object service) => Register(service, service.GetType());

    public void Register(object service, Type contract)
    {
      Locator.Set(contract, service);
      
      if(service is IInitializable initializable)
        _initializables.Add(initializable);
      if(service is ITickable tickable)
        _tickables.Add(tickable);
    }

    private void Awake()
    {
      foreach (var installer in _installers)
        installer.Install(this);
    }

    private void Start()
    {
      foreach (var initializable in _initializables) 
        initializable.Initialize();
    }

    private void Update()
    {
      foreach (var tickable in _tickables) 
        tickable.Tick();
    }

    private void OnDestroy() => Locator.Clear();
  }
}