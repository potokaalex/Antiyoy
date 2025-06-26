using System;
using System.Collections.Generic;
using Client.Code.Gameplay;
using Client.Code.Services;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Region;
using ClientCode.Utilities.Extensions;
using Leopotam.EcsLite;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class RegionFactory : IInitializable
    {
        private readonly EcsController _ecsController;
        private readonly RegionJoiner _joiner;
        private readonly RegionsContainer _container;
        private readonly Instantiator _instantiator;
        private EcsPool<RegionLink> _linkPool;
        private EcsPool<CellComponent> _cellPool;

        public RegionFactory(EcsController ecsController, RegionJoiner joiner, Instantiator instantiator, RegionsContainer container)
        {
            _instantiator = instantiator;
            _container = container;
            _joiner = joiner;
            _ecsController = ecsController;
        }

        public void Initialize()
        {
            _linkPool = _ecsController.World.GetPool<RegionLink>();
            _cellPool = _ecsController.World.GetPool<CellComponent>();
        }

        public void Create(int cellEntity, RegionType type)
        {
            var region = FindRegionOrCreate(cellEntity, type);
            region.Add(cellEntity);
        }

        public void Destroy(int cellEntity)
        {
            //тут должна происходить магия.
        }
        
        private RegionController FindRegionOrCreate(int entity, RegionType type)
        {
            var neighboursRegions = Utilities.ListPool<RegionController>.Get(); 
            GetNeighboursRegionsWithType(entity, type, neighboursRegions);

            if (neighboursRegions.Count == 0)
                return CreateRegion(entity, type);
            if (neighboursRegions.Count == 1)
                return neighboursRegions[0];

            return _joiner.Join(neighboursRegions);
        }

        private RegionController CreateRegion(int entity, RegionType type)
        {
            var region = _instantiator.Instantiate<RegionController>();
            region.Initialize(entity, type);
            _container.Add(region);
            return region;
        }

        private void GetNeighboursRegionsWithType(int entity, RegionType regionType, List<RegionController> outList)// move to container ?
        {
            outList.Clear();
            
            foreach (var neighbourCell in _cellPool.Get(entity).NeighbourCellEntities)
            {
                if (!_linkPool.Has(neighbourCell))
                    continue;

                var region = _linkPool.Get(neighbourCell).Region;

                if (region.Type == regionType && !outList.Contains(region))
                    outList.Add(region);
            }
        }
    }
}