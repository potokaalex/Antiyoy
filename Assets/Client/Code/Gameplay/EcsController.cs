using Leopotam.EcsLite;

namespace Client.Code.Gameplay
{
    public class EcsController //TODO: rename
    {
        public EcsWorld World { get; private set; } = new();
    }
}