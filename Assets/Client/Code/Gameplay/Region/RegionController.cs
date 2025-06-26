using System.Collections;
using System.Collections.Generic;
using Client.Code.Gameplay;
using ClientCode.Gameplay.Region;
using ClientCode.Utilities;
using ClientCode.Utilities.Extensions;
using Leopotam.EcsLite;
using UnityEngine;

namespace ClientCode.Infrastructure.Installers
{
    public class RegionController
    {
        private readonly EcsController _ecsController;
        private readonly GridController _gridController;
        private EcsPool<RegionLink> _linkPool;

        public RegionController(EcsController ecsController, GridController gridController)
        {
            _gridController = gridController;
            _ecsController = ecsController;
        }

        public RegionType Type { get; private set; }

        public List<int> CellEntities { get; } = new();

        public void Initialize(RegionType type)
        {
            _linkPool = _ecsController.World.GetPool<RegionLink>();
            Type = type;
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
            _gridController.SetColor(cellEntity, GetColor());
        }

        public void Add(List<int> cellEntities)
        {
            for (var i = 0; i < cellEntities.Count; i++) 
                Add(cellEntities[i]);
        }

        public void Remove(int cellEntity)
        {
            _linkPool.Del(cellEntity);
            CellEntities.Remove(cellEntity);
            _gridController.SetColor(cellEntity, Color.black);
        }

        public void Remove(List<int> cellEntities)
        {
            for (var i = 0; i < cellEntities.Count; i++) 
                Remove(cellEntities[i]);
        }

        private Color GetColor()
        {
            if(Type == RegionType.Red)
                return Color.red;
            if(Type == RegionType.Blue)
                return Color.blue;
            return Color.gray;
        }
    }
}