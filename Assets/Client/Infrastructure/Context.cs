using System;
using UnityEngine;

namespace Client.Infrastructure
{
  [DefaultExecutionOrder(-10000)]
  public class Context : MonoBehaviour
  {
    [SerializeField] private MonoInstaller[] _installers;
    private readonly Systems _systems = new();

    public void Register(object service, params Type[] contracts)
    {
      var serviceType = service.GetType();

      foreach (var contract in contracts)
      {
        if (!contract.IsAssignableFrom(serviceType))
          Debug.LogError($"{serviceType} is not implementing {contract}");

        Locator.Add(contract, service);

        if (typeof(IInitializable) == contract) 
          _systems.Add((IInitializable)service);
        if (typeof(ITickable) == contract) 
          _systems.Add((ITickable)service);
      }
    }

    private void Awake()
    {
      foreach (var installer in _installers)
        installer.Install(this);
    }

    private void Start() => _systems.Initialize();

    private void Update() => _systems.Tick();

    private void OnDestroy() => Locator.Clear();
  }
}