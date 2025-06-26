using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Client.Code.Gameplay.Cell
{
    public struct CellComponent
    {
        public int Id;
        public List<int> NeighbourCellEntities;
        public Vector2Int GridPosition;
        [CanBeNull] public CellDebugBehaviour Debug;
    }
}