using System.Collections.Generic;
using Client.Code.Core;

namespace Client.Code.Gameplay.Region
{
    public class RegionCreator
    {
        private readonly Instantiator _instantiator;

        public RegionCreator(Instantiator instantiator) => _instantiator = instantiator;

        public void Create(int entity, RegionType type)
        {
            var region = CreateEmpty(type);
            region.Add(entity);
        }

        public void Create(List<int> entity, RegionType type)
        {
            var region = CreateEmpty(type);
            region.Add(entity);
        }

        private RegionController CreateEmpty(RegionType type)
        {
            var region = _instantiator.Instantiate<RegionController>();
            region.Initialize(type);
            return region;
        }
    }
}