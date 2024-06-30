using Code.Cell;
using SevenBoldPencil.EasyEvents;

namespace Code.Tile
{
    public struct TileDestroyRequest : IEventReplicant
    {
        public CellObject Cell;
    }
}