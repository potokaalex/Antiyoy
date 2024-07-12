using Code.Gameplay.Cell;
using SevenBoldPencil.EasyEvents;

namespace Code.Gameplay.Tile.Components
{
    public struct TileCreateRequest : IEventReplicant
    {
        public CellObject Cell;
    }
}