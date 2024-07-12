using System.Collections.Generic;
using ClientCode.Gameplay.Hex;

namespace ClientCode.Gameplay.Cell
{
    public struct CellComponent
    {
        public List<int> NeighbourCellEntities;
        public CellObject Object;
        public HexCoordinates Hex;
    }
}