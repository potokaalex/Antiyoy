using SevenBoldPencil.EasyEvents;

namespace Code.Region.Components
{
    public struct RegionRemoveCellRequest : IEventReplicant
    {
        public int CellEntity;
    }
}