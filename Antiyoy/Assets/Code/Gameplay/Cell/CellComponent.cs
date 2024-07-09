using System.Collections.Generic;
using Code.Gameplay.Hex;

namespace Code.Gameplay.Cell
{
    public struct CellComponent
    {
        public List<int> NeighbourCellEntities;
        public CellObject Object;
        public HexCoordinates Hex;
    }
}