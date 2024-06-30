using System.Collections.Generic;
using Code.Hex;

namespace Code.Cell
{
    public struct CellComponent
    {
        public List<int> NeighboursCellsEntities;
        public HexCoordinates Hex;
    }
}