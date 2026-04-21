using System;
using System.Collections.Generic;
using Client.New.Configs;
using Client.New.Hex;
using Client.New.Infrastructure;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.New.Region
{
  public class RegionsService : IInitializable
  {
    private readonly RegionParts _regionPartsBuffer = new();
    private GridController _gridController;
    private ConfigsProvider _configsProvider;
    private RegionsFactory _regionsFactory;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _configsProvider = Locator.Get<ConfigsProvider>();
      _regionsFactory = Locator.Get<RegionsFactory>();
    }

    public void TryJoinRegions(HexCoordinates position, RegionType type)
    {
      var regions = new List<RegionController>();
      FindRegionsWhitOneType(position, regions, type);
      SortByDecreasing(regions, x => x.Cells.Count);
      JoinRegions(regions);
    }

    public void TryDivideRegion(RegionController region)
    {
      using (ListPool<CellController>.Get(out var unPassed))
      using (ListPool<CellController>.Get(out var front))
      {
        var regionParts = _regionPartsBuffer;
        unPassed.AddRange(region.Cells);
        FindRegionParts(unPassed, front, regionParts);
        SortByDecreasing(regionParts.Items, x => x.Count);
        DivideRegion(regionParts, region);
        regionParts.Clear();
      }
    }

    public void RemoveFromRegionAndTryDivideRegion(CellController cell)
    {
      var region = cell.Region;
      cell.Region.Remove(cell);
      TryDivideRegion(region);
    }

    public Color GetColorFor(RegionController region)
    {
      if (region == null)
        return Color.black;
      return _configsProvider.RegionsColors[region.Type];
    }

    public void AddToBestNeighbourRegion(HexCoordinates position, RegionType type, CellController cell)
    {
      RegionController region = null;

      foreach (var neighbour in _gridController.GetNeighbourCells(position))
        if (neighbour.Region.Type == type && (region == null || neighbour.Region.Cells.Count > region.Cells.Count))
          region = neighbour.Region;

      if (region != null)
        region.Add(cell);
      else
        _regionsFactory.Create(cell, type);
    }

    private void FindRegionCells(List<CellController> front, List<CellController> regionCells, List<CellController> unPassed, bool byType = true)
    {
      while (front.Count > 0)
      {
        var cell = front[0];
        var position = cell.Position;

        foreach (var neighbour in _gridController.GetNeighbourCells(position))
        {
          var isNiceRegion = byType ? neighbour.Region.Type == cell.Region.Type : neighbour.Region == cell.Region;

          if (isNiceRegion && !front.Contains(neighbour) && !regionCells.Contains(neighbour))
          {
            front.Add(neighbour);
            regionCells.Add(neighbour);
          }
        }

        front.RemoveAt(0);
        unPassed.Remove(cell);
      }
    }

    private void FindRegionParts(List<CellController> unPassed, List<CellController> front, RegionParts regionParts)
    {
      using (ListPool<CellController>.Get(out var regionPart))
      {
        while (unPassed.Count > 0)
        {
          var cell = unPassed[0];
          regionPart.Add(cell);
          front.Add(cell);
          FindRegionCells(front, regionPart, unPassed);
          regionParts.NewPartFrom(regionPart);
        }
      }
    }

    private void JoinRegions(List<RegionController> regions)
    {
      var mainRegion = regions[0];
      for (var i = 1; i < regions.Count; i++)
      {
        var region = regions[i];
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

    private void SortByDecreasing<T>(List<T> list, Func<T, int> getValue)
    {
      for (var i = 0; i < list.Count - 1; i++)
      for (var j = 0; j < list.Count - i - 1; j++)
        if (getValue(list[j]) < getValue(list[j + 1]))
          (list[j], list[j + 1]) = (list[j + 1], list[j]);
    }

    private void FindRegionsWhitOneType(HexCoordinates position, List<RegionController> list, RegionType type)
    {
      if (_gridController.TryGetCell(position, out var cell))
        if (cell.Region.Type == type)
          list.Add(cell.Region);

      foreach (var neighbour in _gridController.GetNeighbourCells(position))
        if (neighbour.Region.Type == type && !list.Contains(neighbour.Region))
          list.Add(neighbour.Region);
    }
  }
}