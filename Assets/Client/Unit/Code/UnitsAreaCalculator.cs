using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using Client.Utilities;

namespace Client.Unit.Code
{
  public class UnitsAreaCalculator : IInitializable
  {
    private GridController _gridController;

    public void Initialize() => _gridController = Locator.Get<GridController>();

    public void GetCreationArea(RegionController region, List<CellController> outResult, UnitType unitType)
    {
      using (StackPool<CellController>.Get(out var front))
      {
        outResult.Clear();

        if (unitType == UnitType.Farm)
          GetFarmCreationArea(region, front, outResult);
        else if (unitType == UnitType.Tower)
          outResult.AddRange(region.Cells);
        else
          GetDefaultCreationArea(region, front, outResult);
      }
    }

    public void GetMoveArea(IUnit unit, List<CellController> outList)
    {
      using (QueuePool<UnitMoveAreaCell>.Get(out var front))
      {
        outList.Clear();
        front.Enqueue(new UnitMoveAreaCell(unit.Cell, 4));
        outList.Add(unit.Cell);

        while (front.Count > 0)
        {
          var areaCell = front.Dequeue();

          if (areaCell.RemainingMove == 0)
            continue;

          foreach (var neighbour in _gridController.GetNeighbourCells(areaCell.Cell.Position))
          {
            if (outList.Contains(neighbour))
              continue;

            if (neighbour.Region.Type == unit.Cell.Region.Type)
            {
              outList.Add(neighbour);
              front.Enqueue(new UnitMoveAreaCell(neighbour, areaCell.RemainingMove - 1));
            }
            else
            {
              outList.Add(neighbour);
            }
          }
        }
      }
    }

    public void GetProtectionArea(IUnit unit, List<CellController> outList, bool withRegionCheck)
    {
      outList.Clear();
      outList.Add(unit.Cell);
      foreach (var cell in _gridController.GetNeighbourCells(unit.Cell.Position))
        if (!withRegionCheck || cell.Region.Type == unit.Cell.Region.Type)
          outList.Add(cell);
    }

    private void GetFarmCreationArea(RegionController region, Stack<CellController> front, List<CellController> outResult)
    {
      foreach (var cell in region.Cells)
      {
        if (cell.HasUnit && cell.Unit.Type is UnitType.Capital or UnitType.Farm)
        {
          front.Push(cell);
          outResult.Add(cell);
        }
      }

      while (front.Count > 0)
      {
        var cell = front.Pop();
        foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
          if (neighbour.Region.Type == cell.Region.Type && !outResult.Contains(neighbour))
            outResult.Add(neighbour);
      }
    }

    private void GetDefaultCreationArea(RegionController region, Stack<CellController> front, List<CellController> outResult)
    {
      foreach (var cell in region.Cells)
      {
        front.Push(cell);
        outResult.Add(cell);
      }

      while (front.Count > 0)
      {
        var cell = front.Pop();
        foreach (var neighbour in _gridController.GetNeighbourCells(cell.Position))
          if (neighbour.Region.Type != cell.Region.Type && !outResult.Contains(neighbour))
            outResult.Add(neighbour);
      }
    }
  }
}