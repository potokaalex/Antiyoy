using Leopotam.EcsLite;

namespace Client.Code.Gameplay
{
    public class EcsController //TODO: rename to: ecs provider
    {
        public EcsWorld World { get; private set; } = new();
    }
}