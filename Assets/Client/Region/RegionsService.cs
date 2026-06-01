using System.Collections.Generic;
using Client.Configs;
using Client.Hex;
using Client.Infrastructure;
using Client.Unit.Code;
using Client.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Region
{
  public class RegionsService : IInitializable
  {
    private readonly RegionParts _regionPartsBuffer = new();
    private GridController _gridController;
    private ConfigsProvider _configsProvider;
    private RegionsFactory _regionsFactory;

    public IReadOnlyList<RegionController> Regions => _regionsFactory.ActiveRegions;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _configsProvider = Locator.Get<ConfigsProvider>();
      _regionsFactory = Locator.Get<RegionsFactory>();
    }

    public void RemoveFromRegion(CellController cell)
    {
      var region = cell.Region;
      cell.Region.Remove(cell);
      TryDivideRegion(region);
    }

    public void AddToBestNeighbourRegion(RegionType type, CellController cell)
    {
      RegionController region = null;

      foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
        if (neighbour.Region.Type == type && (region == null || GetRegionPower(neighbour.Region) > GetRegionPower(region)))
          region = neighbour.Region;

      if (region != null)
        region.Add(cell);
      else
        _regionsFactory.Create(cell, type);

      TryJoinRegions(cell.Position, type);
    }

    public Color GetColorFor(RegionController region)
    {
      if (region == null)
        return Color.black;
      return _configsProvider.RegionsColors[region.Type];
    }

    private void TryJoinRegions(HexCoordinates position, RegionType type)
    {
      var regions = new List<RegionController>();
      FindRegionsWhitOneType(position, regions, type);
      regions.SortByDecreasing(GetRegionPower);
      JoinRegions(regions);
    }

    private void TryDivideRegion(RegionController region)
    {
      using (ListPool<CellController>.Get(out var unPassed))
      using (StackPool<CellController>.Get(out var front))
      {
        var regionParts = _regionPartsBuffer;
        unPassed.AddRange(region.Cells);
        FindRegionParts(unPassed, front, regionParts);
        regionParts.Items.SortByDecreasing(GetRegionPower);
        DivideRegion(regionParts, region);
        regionParts.Clear();
      }
    }

    private void FindRegionCells(Stack<CellController> front, List<CellController> regionCells, List<CellController> unPassed)
    {
      while (front.Count > 0)
      {
        var cell = front.Pop();
        var position = cell.Position;

        foreach (var neighbour in _gridController.GetNeighbourCells(position))
        {
          var isNiceRegion = neighbour.Region != null && neighbour.Region.Type == cell.Region.Type;

          if (isNiceRegion && !front.Contains(neighbour) && !regionCells.Contains(neighbour))
          {
            front.Push(neighbour);
            regionCells.Add(neighbour);
          }
        }

        unPassed.Remove(cell);
      }
    }

    private void FindRegionParts(List<CellController> unPassed, Stack<CellController> front, RegionParts regionParts)
    {
      using (ListPool<CellController>.Get(out var regionPart))
      {
        while (unPassed.Count > 0)
        {
          var cell = unPassed[0];
          regionPart.Add(cell);
          front.Push(cell);
          FindRegionCells(front, regionPart, unPassed);
          regionParts.NewPartFrom(regionPart);
          regionPart.Clear();
        }
      }
    }

    private void JoinRegions(List<RegionController> regions)
    {
      var mainRegion = regions[0];
      for (var i = 1; i < regions.Count; i++)
      {
        var region = regions[i];
        mainRegion.Money += region.Money;

        while (region.Cells.Count > 0)
        {
          var cell = region.Cells[0];
          region.Remove(cell);
          mainRegion.Add(cell);
        }
      }
    }

    private void DivideRegion(RegionParts regionParts, RegionController region)
    {
      for (var i = 1; i < regionParts.Items.Count; i++)
      {
        foreach (var cell in regionParts.Items[i])
          region.Remove(cell);

        _regionsFactory.Create(regionParts.Items[i], region.Type);
      }
    }

    private void FindRegionsWhitOneType(HexCoordinates position, List<RegionController> list, RegionType type)
    {
      if (_gridController.GetCell(position, out var cell))
        if (cell.Region.Type == type)
          list.Add(cell.Region);

      foreach (var neighbour in _gridController.GetNeighbourCells(position))
        if (neighbour.Region.Type == type && !list.Contains(neighbour.Region))
          list.Add(neighbour.Region);
    }

    private int GetRegionPower(RegionController region) => GetRegionPower(region.Cells);

    private int GetRegionPower(IReadOnlyList<CellController> cells)
    {
      var result = 0;

      foreach (var cell in cells)
      {
        result++;

        if (cell.Unit && cell.Unit.Type == UnitType.Farm)
          result++;
      }

      return result;
    }
  }
}