using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;
using Client.Unit.Code.Capital;

namespace Client.Gameplay
{
  public class GameplayRegionsController : IInitializable
  {
    private UnitsService _unitsService;
    private RegionsService _regionsService;
    private TreesController _treesController;
    private CapitalsController _capitalsController;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _treesController = Locator.Get<TreesController>();
      _capitalsController = Locator.Get<CapitalsController>();
    }

    public void Enable()
    {
      _regionsService.OnRegionAddCell += UpdateBuildings;
      _regionsService.OnRegionRemoveCell += UpdateBuildings;
    }

    public void Disable()
    {
      _regionsService.OnRegionAddCell -= UpdateBuildings;
      _regionsService.OnRegionRemoveCell -= UpdateBuildings;
    }

    public void Update(RegionController region)
    {
      if (region.Cells.Count <= 1)
      {
        region.Money = 0;
        DestroyAllUnits(region);
      }

      region.Money += region.GetIncome();
      if (region.Money < 0)
      {
        region.Money = 0;
        DestroyAllUnits(region);
      }

      foreach (var cell in region.Cells)
        if (cell.HasUnit)
          cell.Unit.ResetTurns();
    }

    private void UpdateBuildings(RegionController region)
    {
      if (region.Cells.Count <= 1)
        DestroyBuildings(region);
      else
        _capitalsController.CreateCapital(region);
    }

    private void DestroyBuildings(RegionController region)
    {
      foreach (var cell in region.Cells)
      {
        if (cell.HasUnit && cell.Unit.Type.IsBuilding())
        {
          if (cell.Unit.Type == UnitType.Capital)
            _unitsService.Create(cell, _treesController.GetCreationTreeType(cell));
          else
            _unitsService.Destroy(cell.Unit);
        }
      }
    }

    private void DestroyAllUnits(RegionController region)
    {
      foreach (var cell in region.Cells)
      {
        if (cell.HasUnit && cell.Unit.Type.IsWarrior())
        {
          _unitsService.Destroy(cell.Unit);
          _unitsService.Create(cell, UnitType.Grave);
        }
      }
    }
  }
}