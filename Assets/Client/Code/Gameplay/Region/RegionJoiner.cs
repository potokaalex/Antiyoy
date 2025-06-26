using System.Collections.Generic;

namespace Client.Code.Gameplay.Region
{
    public class RegionJoiner
    {
        //moves data to a major region from non-major regions. Deletes non-major regions.
        public RegionController Join(List<RegionController> regions)
        {
            var majorRegion = GetMajorRegion(regions);

            foreach (var region in regions)
            {
                if (region == majorRegion)
                    continue;

                majorRegion.Add(region.CellEntities);
            }

            return majorRegion;
        }

        private RegionController GetMajorRegion(List<RegionController> regions)
        {
            var majorRegion = regions[0];

            for (var i = 1; i < regions.Count; i++)
            {
                var region = regions[i];

                if (region.CellEntities.Count > majorRegion.CellEntities.Count)
                    majorRegion = regions[i];
            }

            return majorRegion;
        }
    }
}