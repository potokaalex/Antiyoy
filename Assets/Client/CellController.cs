using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.Tile;
using Client.Unit.Code;

namespace Client
{
  public class CellController
  {
    private readonly List<IUnitController> _protectionsUnit = new();
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

    public IUnitController Unit { get; set; }

    public bool HasUnit => Unit != null;

    public int Protection
    {
      get
      {
        var max = 0;
        foreach (var unit in _protectionsUnit)
          if (unit.Cell.Region.Type == Region.Type && unit.Protection > max)
            max = unit.Protection;
        return max;
      }
    }

    public void Initialize(HexCoordinates position, RegionType type)
    {
      _tilemapController = Locator.Get<TilemapController>();
      _regionsService = Locator.Get<RegionsService>();
      _unitsService = Locator.Get<UnitsService>();
      Position = position;
      _regionsService.AddToBestNeighbourRegion(type, this);
    }

    public void Dispose()
    {
      _regionsService.RemoveFromRegion(this);
      _unitsService.Destroy(Unit);
    }

    public void ChangeRegionType(RegionType type)
    {
      _regionsService.RemoveFromRegion(this);
      _regionsService.AddToBestNeighbourRegion(type, this);
    }

    public void AddUnitForProtection(IUnitController unit) => _protectionsUnit.Add(unit);

    public void RemoveUnitForProtection(IUnitController unit) => _protectionsUnit.Remove(unit);
  }
}