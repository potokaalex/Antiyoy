using System.Collections.Generic;

namespace ClientCode.Infrastructure.Installers
{
    public class RegionsContainer
    {
        private readonly List<RegionController> _regions = new();

        public void Add(RegionController region) => _regions.Add(region);

        public void Remove(RegionController region)
        {
            region.Dispose(); //i do it in container? why dont in factory ?
            _regions.Remove(region);
        }
    }
}