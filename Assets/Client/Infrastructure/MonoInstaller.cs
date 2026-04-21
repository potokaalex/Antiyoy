using UnityEngine;
using UnityEngine.Pool;

namespace Client.Infrastructure
{
  [DefaultExecutionOrder(-10000)]
  public abstract class MonoInstaller : MonoBehaviour
  {
    private void Awake()
    {
      Install();
    }

    private void Start()
    {
      using var d = ListPool<IInitializable>.Get(out var initializables);
      Locator.GetAll(initializables);
      foreach (var initializable in initializables) 
        initializable.Initialize();
    }
    
    private void OnDestroy()
    {
      Locator.Clear();
    }

    protected virtual void Install()
    {
    }
  }
}