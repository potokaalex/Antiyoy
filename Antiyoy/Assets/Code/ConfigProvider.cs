using System;
using Code.Cell;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class ConfigProvider
    {
        [SerializeField] private CellConfig _cellConfig;
        
        public CellConfig GetCell() => _cellConfig;
    }
}