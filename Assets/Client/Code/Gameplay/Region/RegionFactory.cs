using System.Collections.Generic;
using Client.Code.Gameplay.Cell;
using Leopotam.EcsLite;
using Zenject;

namespace Client.Code.Gameplay.Region
{
    public class RegionFactory : IInitializable
    {
        private readonly EcsController _ecsController;
        private readonly RegionJoiner _joiner;
        private EcsPool<RegionLink> _linkPool;
        private EcsPool<CellComponent> _cellPool;
        private readonly RegionDivider _divider;
        private readonly RegionCreator _creator;

        public RegionFactory(EcsController ecsController, RegionJoiner joiner, RegionDivider divider,
            RegionCreator creator)
        {
            _creator = creator;
            _divider = divider;
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
            var neighboursRegions = Core.ListPool<RegionController>.Get();
            GetNeighboursRegionsWithType(cellEntity, type, neighboursRegions);

            if (neighboursRegions.Count == 0)
                _creator.Create(cellEntity, type);
            else if (neighboursRegions.Count == 1)
                neighboursRegions[0].Add(cellEntity);
            else
                _joiner.Join(neighboursRegions)
                    .Add(cellEntity); //тут должно быть также как с divide, т.е. я должен добавить рег а потом объединить (?)
        }

        public void Destroy(int cellEntity)
        {
            if (!_linkPool.Has(cellEntity))
                return;

            var baseRegion = _linkPool.Get(cellEntity).Region;
            baseRegion.Remove(cellEntity);
            _divider.Divide(baseRegion);
        }

        private void GetNeighboursRegionsWithType(int entity, RegionType regionType, List<RegionController> outList) // move to container ?
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