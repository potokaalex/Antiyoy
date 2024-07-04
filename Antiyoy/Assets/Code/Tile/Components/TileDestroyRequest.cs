using Code.Cell;
using SevenBoldPencil.EasyEvents;

namespace Code.Tile.Components
{
    public struct TileDestroyRequest : IEventReplicant
    {
        public CellObject Cell;
    }
}