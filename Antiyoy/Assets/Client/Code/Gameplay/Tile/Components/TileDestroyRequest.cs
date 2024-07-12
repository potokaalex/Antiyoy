using Code.Gameplay.Cell;
using SevenBoldPencil.EasyEvents;

namespace Code.Gameplay.Tile.Components
{
    public struct TileDestroyRequest : IEventReplicant
    {
        public CellObject Cell;
    }
}