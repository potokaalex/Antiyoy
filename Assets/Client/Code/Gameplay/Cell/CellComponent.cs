using System.Collections.Generic;
using UnityEngine;

namespace ClientCode.Gameplay.Cell
{
    public struct CellComponent
    {
        public int Id;
        public List<int> NeighbourCellEntities;
        public Vector2Int GridPosition;
        public CellDebugBehaviour Debug;
    }
}