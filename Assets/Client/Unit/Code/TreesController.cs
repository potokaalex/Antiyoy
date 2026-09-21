using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using Client.Utilities;
using UnityEngine;

namespace Client.Unit.Code
{
  public class TreesController : IInitializable
  {
    private GridController _gridController;
    private UnitsService _unitsService;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
    }

    public void UpdateTrees()
    {
      SpawnPines();
      SpawnPalms();
    }

    public void UpdateGraves(RegionController region)
    {
      foreach (var cell in region.Cells)
      {
        if (cell.HasUnit && cell.Unit.Type == UnitType.Grave)
        {
          _unitsService.Destroy(cell.Unit);
          _unitsService.Create(cell, GetCreationTreeType(cell));
        }
      }
    }

    public UnitType GetCreationTreeType(CellController cell) => CountCellsAround(cell) != 6 ? UnitType.Palm : UnitType.Pine;

    private void SpawnPines()
    {
      GameConstants.AreaBuffer.Clear();
      foreach (var unit in _unitsService.Units)
        if (unit.Type == UnitType.Pine)
          MarkPines(unit, GameConstants.AreaBuffer);
      Spawn(UnitType.Pine, GameConstants.AreaBuffer);
    }

    private void SpawnPalms()
    {
      GameConstants.AreaBuffer.Clear();
      foreach (var unit in _unitsService.Units)
        if (unit.Type == UnitType.Palm)
          MarkPalms(unit, GameConstants.AreaBuffer);
      Spawn(UnitType.Palm, GameConstants.AreaBuffer);
    }

    private void Spawn(UnitType unitType, List<CellController> buffer)
    {
      foreach (var cell in buffer)
        if (Random.value < 0.3333)
          _unitsService.Create(cell, unitType, false);
    }

    private void MarkPines(IUnit unit, List<CellController> buffer)
    {
      foreach (var cell in _gridController.GetNeighbourCells(unit.Cell.Position))
        if (!cell.HasUnit && CountTreesAround(cell) >= 2 && !buffer.Contains(cell))
          buffer.Add(cell);
    }

    private void MarkPalms(IUnit unit, List<CellController> buffer)
    {
      foreach (var cell in _gridController.GetNeighbourCells(unit.Cell.Position))
        if (!cell.HasUnit && CountCellsAround(cell) != 6 && !buffer.Contains(cell))
          buffer.Add(cell);
    }

    private int CountTreesAround(CellController cell)
    {
      var result = 0;
      foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
        if (neighbour.HasUnit && neighbour.Unit.Type.IsTree())
          result++;
      return result;
    }

    private int CountCellsAround(CellController cell)
    {
      var result = 0;
      foreach (var unused in _gridController.GetNeighbourCells(cell.Position))
        result++;
      return result;
    }
  }
}