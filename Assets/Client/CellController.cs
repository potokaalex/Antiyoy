using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.Tile;
using Client.Unit;

namespace Client
{
  public class CellController
  {
    private TilemapController _tilemapController;
    private RegionsService _regionsService;
    private RegionController _region;
    private UnitsService _unitsService;

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

    public UnitController Unit { get; set; }

    public void Initialize(HexCoordinates position, RegionType type)
    {
      _tilemapController = Locator.Get<TilemapController>();
      _regionsService = Locator.Get<RegionsService>();
      _unitsService = Locator.Get<UnitsService>();
      Position = position;
      _regionsService.AddToBestNeighbourRegion(position, type, this);
    }

    public void Dispose()
    {
      _regionsService.RemoveFromRegionAndTryDivideRegion(this);
      _unitsService.TryDestroy(Unit);
    }
  }
}