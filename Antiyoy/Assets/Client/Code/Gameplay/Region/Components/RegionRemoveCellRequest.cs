using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Region.Components
{
    public struct RegionRemoveCellRequest : IEventReplicant
    {
        public int CellEntity;
    }
}