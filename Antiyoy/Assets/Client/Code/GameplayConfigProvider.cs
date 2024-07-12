using System;
using ClientCode.Data.Configs;
using ClientCode.Gameplay;
using ClientCode.Gameplay.Cell;
using UnityEngine;

namespace ClientCode.Infrastructure.Gameplay
{
    [Serializable]
    public class GameplayConfigProvider
    {
        [SerializeField] private CellConfig _cellConfig;
        [SerializeField] private MapConfig _mapConfig;
        
        public CellConfig GetCell() => _cellConfig;

        public MapConfig GetMap() => _mapConfig;
    }
}