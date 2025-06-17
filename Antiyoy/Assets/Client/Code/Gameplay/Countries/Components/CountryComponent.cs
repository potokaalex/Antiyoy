using System.Collections.Generic;
using ClientCode.Gameplay.Region;

namespace ClientCode.Gameplay.Countries.Components
{
    public struct CountryComponent
    {
        public List<int> RegionsEntities;
        public RegionType Type;
    }
}