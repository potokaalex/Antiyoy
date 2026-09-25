using System;
using System.Collections.Generic;
using Client.Borders;
using Client.Hex;
using Client.Infrastructure;
using Client.Unit.Code;
using Client.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Region
{
  public class RegionsService : SerializedMonoBehaviour, IInitializable
  {
    private readonly RegionParts _regionPartsBuffer = new();
    [SerializeField] private Dictionary<RegionType, Color> _regionsColors;
    private GridController _gridController;
    private RegionsFactory _regionsFactory;
    private BordersService _bordersService;

    public event Action<RegionController> OnRegionAddCell;
    
    public event Action<RegionController> OnRegionRemoveCell;

    public IReadOnlyList<RegionController> Regions => _regionsFactory.ActiveRegions;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _regionsFactory = Locator.Get<RegionsFactory>();
      _bordersService = Locator.Get<BordersService>();
    }

    public void InitialCreateRegions()
    {
      List<List<CellController>> regions = new() { new(), new(), new() };

      for (var y = 0; y < _gridController.Size.y; y++)
      for (var x = 0; x < _gridController.Size.x; x++)
      {
        _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)), out var cell);

        if (y == 0)
        {
          if (x < 4)
            regions[0].Add(cell);
          else if (x <= 9)
            regions[1].Add(cell);
        }
        else
          regions[2].Add(cell);
      }

      _regionsFactory.Create(regions[0], AddCell, RegionType.Red);
      _regionsFactory.Create(regions[1], AddCell, RegionType.Blue);
      _regionsFactory.Create(regions[2], AddCell);

      foreach (var region in Regions)
        region.Money = 100;
    }

    public void Clear()
    {
      for (var i = Regions.Count - 1; i >= 0; i--) 
        _regionsFactory.Destroy(Regions[i]);
    }

    public void RemoveFromRegion(CellController cell)
    {
      var region = cell.Region;
      RemoveCell(region, cell);
      TryDivideRegion(region);
    }

    public void AddToBestNeighbourRegion(RegionType type, CellController cell)
    {
      RegionController region = null;

      foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
        if (neighbour.Region.Type == type && (region == null || GetRegionPower(neighbour.Region) > GetRegionPower(region)))
          region = neighbour.Region;

      if (region != null)
        AddCell(region, cell);
      else
        _regionsFactory.Create(cell, AddCell, type);

      TryJoinRegions(cell.Position, type);
      _bordersService.ViewRegionsBorders();
    }

    public Color GetColorFor(RegionController region)
    {
      if (region == null)
        return Color.black;
      return _regionsColors[region.Type];
    }

    public void SetRegionType(CellController cell, RegionType type)
    {
      if (cell.Region.Type == type)
        return;

      RemoveFromRegion(cell);
      AddToBestNeighbourRegion(type, cell);
    }

    public void GetUniqueRegionsAtAndAround(CellController cell, List<RegionController> outList)
    {
      outList.Add(cell.Region);

      foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
        if (!outList.Contains(neighbour.Region))
          outList.Add(neighbour.Region);
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
          RemoveCell(region, cell);
          AddCell(mainRegion, cell);
        }
      }
    }

    private void DivideRegion(RegionParts regionParts, RegionController region)
    {
      for (var i = 1; i < regionParts.Items.Count; i++)
      {
        foreach (var cell in regionParts.Items[i])
          RemoveCell(region, cell);

        _regionsFactory.Create(regionParts.Items[i], AddCell, region.Type);
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

        if (cell.HasUnit && cell.Unit.Type == UnitType.Farm)
          result++;
      }

      return result;
    }

    private void AddCell(RegionController region, CellController cell)
    {
      region.Add(cell);
      OnRegionAddCell?.Invoke(region);
    }

    private void RemoveCell(RegionController region, CellController cell)
    {
      region.Remove(cell);
      OnRegionRemoveCell?.Invoke(region);
    }
  }
}