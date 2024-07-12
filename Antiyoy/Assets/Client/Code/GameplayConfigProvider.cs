using System;
using ClientCode.Gameplay.Cell;
using UnityEngine;

namespace ClientCode
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