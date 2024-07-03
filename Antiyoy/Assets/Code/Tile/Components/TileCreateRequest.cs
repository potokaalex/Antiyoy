using Code.Cell;
using SevenBoldPencil.EasyEvents;

namespace Code.Tile.Components
{
    public struct TileCreateRequest : IEventReplicant
    {
        public CellObject Cell;
    }
}