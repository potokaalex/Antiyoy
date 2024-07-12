using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Region.Components
{
    public struct RegionAddCellRequest : IEventReplicant
    {
        public int CellEntity;
    }
}