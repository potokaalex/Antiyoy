using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace Code.Gameplay.Ecs
{
    public interface IEcsProvider
    {
        void Initialize(EcsWorld world, EventsBus eventBus, IEcsSystems systems);
        EcsWorld GetWorld();
        EventsBus GetEventsBus();
        IEcsSystems GetSystems();
    }
}