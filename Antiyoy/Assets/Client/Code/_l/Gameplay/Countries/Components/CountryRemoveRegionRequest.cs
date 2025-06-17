using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Countries.Components
{
    public struct CountryRemoveRegionRequest : IEventReplicant
    {
        public int RegionEntity;
    }
}