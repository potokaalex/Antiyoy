using SevenBoldPencil.EasyEvents;

namespace Code.Cell
{
    public struct CellCreateRequest : IEventReplicant
    {
        public int Index;
    }
}