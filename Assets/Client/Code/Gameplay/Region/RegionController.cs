using System.Collections;
using System.Collections.Generic;
using Client.Code.Gameplay;
using ClientCode.Gameplay.Region;
using ClientCode.Utilities;
using ClientCode.Utilities.Extensions;
using Leopotam.EcsLite;

namespace ClientCode.Infrastructure.Installers
{
    public class RegionController
    {
        private readonly EcsController _ecsController;
        private EcsPool<RegionLink> _linkPool;

        public RegionController(EcsController ecsController) => _ecsController = ecsController;

        public RegionType Type { get; private set; }

        public List<int> CellEntities { get; private set; }

        public void Initialize(int cellEntity, RegionType type)
        {
            _linkPool = _ecsController.World.GetPool<RegionLink>();
            Type = type;
            CellEntities = new List<int>(cellEntity);
        }
        
        public void Dispose()
        {
            var buffer = ListPool<int>.Get();
            buffer.AddRange(CellEntities);
            
            for (var i = 0; i < buffer.Count; i++) 
                Remove(buffer[i]);
            
            ListPool<int>.Release(buffer);
        }

        public void Add(int cellEntity)
        {
            CellEntities.Add(cellEntity);
            ref var link = ref _linkPool.GetOrAdd(cellEntity);
            link.Region = this;
        }

        public void Remove(int cellEntity)
        {
            _linkPool.Del(cellEntity);
            CellEntities.Remove(cellEntity);
        }
        
        public void Add(List<int> cellEntities)
        {
            for (var i = 0; i < cellEntities.Count; i++) 
                Add(cellEntities[i]);
        }
    }
}