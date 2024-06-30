using Code.Cell;
using SevenBoldPencil.EasyEvents;

namespace Code.Tile
{
    public struct TileCreateRequest : IEventReplicant
    {
        public CellObject Cell;
    }
}