using SevenBoldPencil.EasyEvents;

namespace Code.Gameplay.Region.Components
{
    public struct RegionAddCellRequest : IEventReplicant
    {
        public int CellEntity;
    }
}