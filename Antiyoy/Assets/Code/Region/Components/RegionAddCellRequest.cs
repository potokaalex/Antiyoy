using SevenBoldPencil.EasyEvents;

namespace Code.Region.Components
{
    public struct RegionAddCellRequest : IEventReplicant
    {
        public int CellEntity;
    }
}