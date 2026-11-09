using Client.Project.Region;

namespace Client.MapEditor
{
  public struct MapEditorPlayerRegionType
  {
    public RegionType RegionType;
    public bool IsRandom;

    public MapEditorPlayerRegionType(RegionType regionType, bool isRandom = false)
    {
      RegionType = regionType;
      IsRandom = isRandom;
    }
  }
}