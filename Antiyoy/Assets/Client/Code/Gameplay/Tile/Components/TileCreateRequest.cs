using ClientCode.Gameplay.Cell;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Tile.Components
{
    public struct TileCreateRequest : IEventReplicant
    {
        public CellObject Cell;
    }
}