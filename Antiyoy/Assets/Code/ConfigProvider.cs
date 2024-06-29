using System;
using Code.Cell;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class ConfigProvider
    {
        [SerializeField] private CellConfig _cellConfig;
        [SerializeField] private MapConfig _mapConfig;
        
        public CellConfig GetCell() => _cellConfig;

        public MapConfig GetMap() => _mapConfig;
    }
}