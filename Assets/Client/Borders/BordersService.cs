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
  //нужно как-то подсветить границы заселекченного региона.
  //и сделать красивую анимацию для заселекченных тайлов.
  //т.е. наверное можно просто расширить интерфейс сервиса, но в нём уже многовато логики, значит можно вынести какую-то фабрику.
  
  public class BordersService : MonoBehaviour, IInitializable
  {
    [SerializeField] private BorderController _borderPrefab;
    [SerializeField] private float _borderWidth;
    [SerializeField] private float _borderHeight;
    private readonly List<BorderController> _activeBorders = new();
    private readonly HashSet<Vector2> _activeBordersPositions = new();
    private RegionsService _regionsService;
    private GridController _gridController;
    private ObjectPool<BorderController> _bordersPool;

    public void Initialize()
    {
      _regionsService = Locator.Get<RegionsService>();
      _gridController = Locator.Get<GridController>();
      _bordersPool = new ObjectPool<BorderController>(CreateBorder, x => x.SetActive(true), x => x.SetActive(false));
    }

    public void ViewSelectedRegionBorders()
    {
      
    }

    public void ViewRegionsBorders()
    {
      ReleaseActiveBorders();

      foreach (var region in _regionsService.Regions)
      {
        foreach (var cell in region.Cells)
        {
          var cellPosition = cell.Position;
          var cellWorldPosition = (Vector2)_gridController.HexPositionToWorld(cell.Position);
          foreach (var direction in HexUtilities.Directions)
          {
            if (_gridController.GetCell(cellPosition + direction, out var neighbourCell))
            {
              if (neighbourCell.Region.Type != region.Type)
                CreateBorder(cellWorldPosition, direction);
            }
            else
              CreateBorder(cellWorldPosition, direction);
          }
        }
      }
    }

    private void ReleaseActiveBorders()
    {
      for (var i = _activeBorders.Count - 1; i >= 0; i--)
      {
        _bordersPool.Release(_activeBorders[i]);
        _activeBorders.RemoveAt(i);
      }

      _activeBordersPositions.Clear();
    }

    private void CreateBorder(Vector2 cellPosition, HexCoordinates direction)
    {
      CalculatePosition(cellPosition, direction, out var position, out var zRotation);
      position = new Vector2(MathF.Round(position.x, 3), MathF.Round(position.y, 3));

      if (_activeBordersPositions.Contains(position))
        return;

      var border = _bordersPool.Get();
      border.Transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, zRotation));
      _activeBorders.Add(border);
      _activeBordersPositions.Add(position);
    }

    private void CalculatePosition(Vector2 cellPosition, HexCoordinates direction, out Vector2 position, out float zRotation)
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

    private BorderController CreateBorder()
    {
      var instance = Instantiate(_borderPrefab, transform);
      instance.Initialize(_borderWidth, _borderHeight);
      return instance;
    }
  }
}