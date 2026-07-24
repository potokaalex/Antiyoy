using System;
using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Client.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Borders
{
  public class BordersFactory : MonoBehaviour
  {
    [SerializeField] private BorderController _prefab;
    private readonly List<BorderController> _active = new();
    private readonly HashSet<Vector2> _activePositions = new();
    private ObjectPool<BorderController> _pool;
    private RegionsService _regionsService;
    private GridController _gridController;

    private void Awake()
    {
      _regionsService = Locator.Get<RegionsService>();
      _gridController = Locator.Get<GridController>();
      _pool =
        new ObjectPool<BorderController>(() => Instantiate(_prefab, transform), x => x.SetActive(true), x => x.SetActive(false));
    }

    public void ViewRegionsBorders()
    {
      ClearBorders();

      foreach (var region in _regionsService.Regions)
      foreach (var cell in region.Cells) 
        CreateAround(cell, region.Type, CreateBorder);
    }

    public void ClearBorders()
    {
      for (var i = _active.Count - 1; i >= 0; i--)
      {
        _pool.Release(_active[i]);
        _active.RemoveAt(i);
      }

      _activePositions.Clear();
    }
    
    public void CreateAround(CellController cell, RegionType regionType, Action<Vector2, HexCoordinates> create)
    {
      var cellWorldPosition = (Vector2)_gridController.HexPositionToWorld(cell.Position);
      foreach (var direction in HexUtilities.Directions)
      {
        if (_gridController.GetCell(cell.Position + direction, out var neighbourCell))
        {
          if (neighbourCell.Region.Type != regionType)
            create(cellWorldPosition, direction);
        }
        else
          create(cellWorldPosition, direction);
      }
    }

    public void CalculatePosition(Vector2 cellPosition, HexCoordinates direction, out Vector2 position, out float zRotation)
    {
      if (direction == HexUtilities.North)
      {
        position = cellPosition + new Vector2(0, MathUtilities.HexHeight);
        zRotation = 0;
      }
      else if (direction == HexUtilities.South)
      {
        position = cellPosition - new Vector2(0, MathUtilities.HexHeight);
        zRotation = 0;
      }
      else if (direction == HexUtilities.Northeast)
      {
        position = cellPosition + new Vector2(MathUtilities.HexHeight * MathUtilities.Sqrt3Div2, MathUtilities.HexHalfHeight);
        zRotation = -60;
      }
      else if (direction == HexUtilities.Southeast)
      {
        position = cellPosition + new Vector2(MathUtilities.HexHeight * MathUtilities.Sqrt3Div2, -MathUtilities.HexHalfHeight);
        zRotation = 60;
      }
      else if (direction == HexUtilities.Southwest)
      {
        position = cellPosition + new Vector2(-MathUtilities.HexHeight * MathUtilities.Sqrt3Div2, -MathUtilities.HexHalfHeight);
        zRotation = -60;
      }
      else if (direction == HexUtilities.Northwest)
      {
        position = cellPosition + new Vector2(-MathUtilities.HexHeight * MathUtilities.Sqrt3Div2, MathUtilities.HexHalfHeight);
        zRotation = 60;
      }
      else
      {
        position = Vector2.zero;
        zRotation = 0;
      }
    }
    
    private void CreateBorder(Vector2 cellPosition, HexCoordinates direction)
    {
      CalculatePosition(cellPosition, direction, out var position, out var zRotation);
      position = new Vector2(MathF.Round(position.x, 3), MathF.Round(position.y, 3));

      if (_activePositions.Add(position))
      {
        var border = _pool.Get();
        border.Transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, zRotation));
        _active.Add(border);
      }
    }
  }
}