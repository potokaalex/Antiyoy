using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Tile.Components
{
    public struct TileCreateRequest : IEventReplicant
    {
        public int CellEntity;
    }
}