using System.Collections.Generic;
using Client.Hex;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Borders
{
  public class TilesSelectionBordersFactory : MonoBehaviour
  {
    [SerializeField] private BordersFactory _bordersFactory;
    [SerializeField] private BorderController _prefab;
    [SerializeField] private float _height;
    [SerializeField] private float _width;
    private readonly List<BorderController> _active = new();
    private ObjectPool<BorderController> _pool;

    private void Awake()
    {
      _pool =
        new ObjectPool<BorderController>(() => Instantiate(_prefab, transform), x => x.SetActive(true), x => x.SetActive(false));
    }

    public void ViewBorders(List<CellController> cells)
    {
      ClearBorders();

      foreach (var cell in cells)
        _bordersFactory.CreateAround(cell, cells, CreateBorder);
    }

    public void ClearBorders()
    {
      DOTween.Kill(this);

      for (var i = _active.Count - 1; i >= 0; i--)
      {
        _pool.Release(_active[i]);
        _active.RemoveAt(i);
      }
    }

    private void CreateBorder(Vector2 cellPosition, HexCoordinates direction)
    {
      _bordersFactory.CalculatePosition(cellPosition, direction, out var position, out var zRotation);
      var border = _pool.Get();
      _active.Add(border);
      border.Transform.rotation = Quaternion.Euler(0, 0, zRotation);
      if (direction == HexUtilities.Southeast || direction == HexUtilities.South || direction == HexUtilities.Southwest)
        border.Transform.localScale = new Vector3(_height, -_width, 1);
      else
        border.Transform.localScale = new Vector3(_height, _width, 1);

      border.DoAppearAnimation(position, (position - cellPosition).normalized).SetId(this);
    }
  }
}