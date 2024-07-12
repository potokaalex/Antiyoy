using ClientCode.Gameplay.Cell;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Tile.Components
{
    public struct TileDestroyRequest : IEventReplicant
    {
        public CellObject Cell;
    }
}