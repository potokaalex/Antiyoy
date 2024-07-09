using SevenBoldPencil.EasyEvents;

namespace Code.Gameplay.Region.Components
{
    public struct RegionRemoveCellRequest : IEventReplicant
    {
        public int CellEntity;
    }
}