using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;

namespace Code
{
    public class EcsFactory
    {
        private EcsWorld _world;
        private IEcsSystems _systems;

        public void Create()
        {
            _systems = new EcsSystems(_world);

            _systems
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsSystemsDebugSystem())
#endif
                .Init();
        }

        private void Start()
        {
            //.Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void Destroy()
        {
            _systems.Destroy();
            _world.Destroy();
        }

        public EcsWorld CreateWorld() => _world = new EcsWorld();
    }
}