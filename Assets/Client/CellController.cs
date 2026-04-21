using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.Tile;

namespace Client
{
  public class CellController
  {
    private TilemapController _tilemapController;
    private RegionsService _regionsService;
    private RegionController _region;

    public HexCoordinates Position { get; private set; }

    public RegionController Region
    {
      get => _region;
      set
      {
        _region = value;
        _tilemapController.SetColor(Position, _regionsService.GetColorFor(_region));
      }
    }

    public void Initialize(HexCoordinates position, RegionType type)
    {
      _tilemapController = Locator.Get<TilemapController>();
      _regionsService = Locator.Get<RegionsService>();
      Position = position;
      _regionsService.AddToBestNeighbourRegion(position, type, this);
    }
  }
}