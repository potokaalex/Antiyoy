﻿using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Ecs
{
    public class EcsProvider : IEcsProvider
    {
        private EcsWorld _world;
        private EventsBus _eventBus;
        private IEcsSystems _systems;

        public void Initialize(EcsWorld world, EventsBus eventBus, IEcsSystems systems)
        {
            _world = world;
            _eventBus = eventBus;
            _systems = systems;
        }

        public EcsWorld GetWorld() => _world;

        public EventsBus GetEventsBus() => _eventBus;

        public IEcsSystems GetSystems() => _systems;
    }
}