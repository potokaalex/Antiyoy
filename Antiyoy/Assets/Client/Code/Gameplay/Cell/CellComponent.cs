using System.Collections.Generic;
using UnityEngine;

namespace ClientCode.Gameplay.Cell
{
    public struct CellComponent
    {
        public List<int> NeighbourCellEntities;
        public Vector2Int GridPosition;
        public int Id;
    }
}