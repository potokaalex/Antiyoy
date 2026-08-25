using System.Collections.Generic;

namespace Client.Infrastructure
{
  public class Systems
  {
    private readonly List<IInitializable> _initializables = new();
    private readonly List<ITickable> _tickables = new();

    public void Add(IInitializable initializable) => _initializables.Add(initializable);

    public void Add(ITickable tickable) => _tickables.Add(tickable);

    public void Initialize()
    {
      foreach (var initializable in _initializables)
        initializable.Initialize();
    }

    public void Tick()
    {
      foreach (var tickable in _tickables)
        tickable.Tick();
    }
  }
}