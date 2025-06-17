using ClientCode.Gameplay.Region;
using SevenBoldPencil.EasyEvents;

namespace ClientCode.Gameplay.Countries.Components
{
    public struct CountryAddRegionRequest : IEventReplicant
    {
        public int RegionEntity;
    }
}