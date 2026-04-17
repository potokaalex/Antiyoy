using System;
using System.Collections.Generic;
using Client.New.Hex;

namespace Client.New.Region
{
  public class RegionsService
  {
    private readonly List<RegionController> _regions = new();
    private readonly GridController _gridController;

    public RegionsService(GridController gridController)
    {
      _gridController = gridController;
    }

    public void CreateRegions()
    {
      var cells = _gridController.Cells;
      if (cells.Length == 0)
      {
        return;
      }

      var unPassed = new List<CellController>(cells);
      var front = new List<CellController>();
      var regionCells = new List<CellController>();

      while (unPassed.Count > 0)
      {
        var baseCell = unPassed[0];
        regionCells.Add(baseCell);
        front.Add(baseCell);
        FindRegionCells(front, regionCells, unPassed);
        _regions.Add(CreateRegion(regionCells));
        regionCells.Clear();
      }
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
      var unPassed = new List<CellController>(region.Cells);
      var front = new List<CellController>();
      var regionParts = new List<List<CellController>>();
      FindRegionParts(unPassed, front, regionParts);
      SortByDecreasing(regionParts, x => x.Count);
      DivideRegion(regionParts, region);
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

    private RegionController CreateRegion(List<CellController> regionCells)
    {
      var region = new RegionController(regionCells);
      foreach (var cell in region.Cells)
      {
        cell.Region = region;
      }

      return region;
    }

    private void FindRegionParts(List<CellController> unPassed, List<CellController> front, List<List<CellController>> regionParts)
    {
      while (unPassed.Count > 0)
      {
        var cell = unPassed[0];
        var regionPart = new List<CellController> { cell };
        front.Add(cell);
        FindRegionCells(front, regionPart, unPassed);
        regionParts.Add(regionPart);
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

    private void DivideRegion(List<List<CellController>> regionParts, RegionController region)
    {
      for (var i = 1; i < regionParts.Count; i++)
      {
        foreach (var cell in regionParts[i])
          region.Remove(cell);

        _regions.Add(new RegionController(regionParts[i]));
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