using System.Collections.Generic;
using Client.Code.Services;
using ClientCode.Gameplay.Region;

namespace ClientCode.Infrastructure.Installers
{
    public class RegionCreator
    {
        private readonly Instantiator _instantiator;
        private readonly RegionsContainer _container;

        public RegionCreator(Instantiator instantiator, RegionsContainer container)
        {
            _instantiator = instantiator;
            _container = container;
        }

        public RegionController Create(int entity, RegionType type)
        {
            var region = CreateEmpty(type);
            region.Add(entity);
            return region;   
        }

        public RegionController Create(List<int> entity, RegionType type)
        {
            var region = CreateEmpty(type);
            region.Add(entity);
            return region;   
        }
        
        private RegionController CreateEmpty(RegionType type)
        {
            var region = _instantiator.Instantiate<RegionController>();
            region.Initialize(type);
            _container.Add(region);
            return region;
        }
    }
}