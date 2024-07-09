using System;
using Code.Gameplay;
using Code.Gameplay.Cell;
using UnityEngine;

namespace Code.Infrastructure.Gameplay
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