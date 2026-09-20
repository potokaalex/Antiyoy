using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;
using Client.Unit.Code.Capital;
using UnityEngine.Pool;

namespace Client.Recovery
{
  public class RecoveryController : IInitializable
  {
    private RegionsService _regionsService;
    private UnitsService _unitsService;
    private CapitalsController _capitalsService;

    public void Initialize()
    {
      _regionsService = Locator.Get<RegionsService>();
      _unitsService = Locator.Get<UnitsService>();
      _capitalsService = Locator.Get<CapitalsController>();
    }

    public RegionsRecoveryData RecoveryRegionsAround(CellController cell)
    {
      var regionsRecoverData = new RegionsRecoveryData
      {
        Regions = ListPool<RegionRecoveryData>.Get()
      };

      using (ListPool<RegionController>.Get(out var regions))
      {
        _regionsService.GetUniqueRegionsAtAndAround(cell, regions);
        foreach (var region in regions)
          regionsRecoverData.Regions.Add(CreateRegionRecoveryData(region));
      }

      return regionsRecoverData;
    }

    public void RestoreRegions(RegionsRecoveryData data)
    {
      foreach (var region in data.Regions)
      {
        foreach (var cell in region.Cells) 
          _regionsService.SetRegionType(cell.Cell, region.Type);

        for (var i = 0; i < region.Cells.Count; i++)
        {
          var cell = region.Cells[i].Cell;
          cell.Region.SetCell(cell, i);
        }

        foreach (var cell in region.Cells)
        {
          _unitsService.Destroy(cell.Cell.Unit);

          if (cell.Unit.HasValue)
          {
            var unit = cell.Unit.Value;
            if (unit.Type == UnitType.Capital)
              _capitalsService.SetCapital(cell.Cell);
            else
              _unitsService.Create(cell.Cell, unit.Type, unit.HasTurns);
          }
        }

        region.Cells[0].Cell.Region.Money = region.Money;
      }

      Dispose(data);
    }

    public void Dispose(RegionsRecoveryData data)
    {
      foreach (var region in data.Regions)
        ListPool<CellRecoveryData>.Release(region.Cells);

      ListPool<RegionRecoveryData>.Release(data.Regions);
    }

    private RegionRecoveryData CreateRegionRecoveryData(RegionController region)
    {
      var cells = ListPool<CellRecoveryData>.Get();
      FillCells(region, cells);
      return new RegionRecoveryData
      {
        Cells = cells,
        Type = region.Type,
        Money = region.Money,
      };
    }

    private void FillCells(RegionController region, List<CellRecoveryData> cells)
    {
      foreach (var cell in region.Cells)
      {
        cells.Add(new CellRecoveryData
        {
          Cell = cell,
          Unit = CreateUnitRecoveryData(cell)
        });
      }
    }

    private UnitRecoveryData? CreateUnitRecoveryData(CellController cell)
    {
      if (cell.HasUnit)
      {
        return new UnitRecoveryData
        {
          Type = cell.Unit.Type,
          HasTurns = cell.Unit.HasTurns
        };
      }

      return null;
    }
  }
}