using ClientCode.Data.Progress.Map;
using ClientCode.Gameplay.Cell;
using ClientCode.Gameplay.Ecs;
using ClientCode.Gameplay.Region.Components;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.Progress.Map.Factory;
using ClientCode.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using SevenBoldPencil.EasyEvents;
using Zenject;

namespace ClientCode.Gameplay.Region
{
    public class RegionFactory : IInitializable, IProgressReader<MapProgressData>, IProgressWriter<MapProgressData>
    {
        private readonly IEcsProvider _ecsProvider;
        private EventsBus _eventsBus;
        private EcsPool<RegionAddCellRequest> _addRequestPool;
        private EcsFilter _addRequestFilter;
        private EcsPool<RegionRemoveCellRequest> _removeRequestPool;
        private EcsFilter _removeRequestFilter;
        private EcsPool<RegionLink> _linkPool;
        private EcsWorld _world;
        private MapProgressData _progress;
        private EcsFilter _filter;
        private EcsPool<RegionComponent> _pool;
        private EcsPool<CellComponent> _cellPool;

        public RegionFactory(IEcsProvider ecsProvider) => _ecsProvider = ecsProvider;

        public void Initialize()
        {
            _world = _ecsProvider.GetWorld();
            _eventsBus = _ecsProvider.GetEventsBus();
            _addRequestFilter = _eventsBus.GetEventBodies(out _addRequestPool);
            _removeRequestFilter = _eventsBus.GetEventBodies(out _removeRequestPool);
            _linkPool = _world.GetPool<RegionLink>();
            _pool = _world.GetPool<RegionComponent>();
            _filter = _world.Filter<RegionComponent>().End();
            _cellPool = _world.GetPool<CellComponent>();
        }

        public void Create(CellObject[] cells)
        {
            foreach (var region in _progress.Regions)
            foreach (var cellId in region.CellsId)
                Create(cells[cellId].Entity, region.Type);
        }

        public void Create(int cell, RegionType type)
        {
            if (_addRequestPool.Has(_addRequestFilter, r => r.CellEntity == cell && r.Type == type))
                return;

            Destroy(cell);

            ref var regionRequest = ref _eventsBus.NewEvent<RegionAddCellRequest>();
            regionRequest.CellEntity = cell;
            regionRequest.Type = type;
        }

        public void Destroy(int cell)
        {
            if (_removeRequestPool.Has(_removeRequestFilter, r => r.CellEntity == cell))
                return;

            if (!_linkPool.Has(cell))
                return;

            ref var regionRequest = ref _eventsBus.NewEvent<RegionRemoveCellRequest>();
            regionRequest.CellEntity = cell;
        }

        public void OnLoad(MapProgressData progress) => _progress = progress;

        public UniTask OnSave(MapProgressData progress)
        {
            progress.Regions.Clear();

            foreach (var entity in _filter)
                progress.Regions.Add(ProgressDataFactory.CreateRegionData(entity, _pool, _cellPool));
            return UniTask.CompletedTask;
        }
    }
}