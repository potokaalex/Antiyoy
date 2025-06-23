using Leopotam.EcsLite;

namespace Client.Code.Gameplay
{
    public class EcsController
    {
        public EcsWorld World { get; private set; } = new();
    }
}