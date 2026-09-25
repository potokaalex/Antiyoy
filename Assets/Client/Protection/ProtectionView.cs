using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;
using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Protection
{
  public class ProtectionView : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _icon;
    private readonly List<ProtectionIconData> _icons = new();
    private GridController _gridController;
    private ObjectPool<SpriteRenderer> _pool;
    private Tween _animation;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      _pool = new ObjectPool<SpriteRenderer>(() => Instantiate(_icon, transform), x => x.enabled = true, x => x.enabled = false);

      _animation = DOTween.Sequence()
        .Append(DOVirtual.Float(0, 1, 0.25f, v =>
        {
          foreach (var icon in _icons)
          {
            icon.Transform.position = Vector3.Lerp(icon.StartPosition, icon.EndPosition, v);
            icon.Renderer.SetAlpha(v);
          }
        }))
        .AppendInterval(0.5f)
        .Append(DOVirtual.Float(0, 1, 0.25f, v =>
        {
          foreach (var icon in _icons)
          {
            icon.Transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.15f, v);
            icon.Renderer.SetAlpha(1 - v);
          }
        })).SetAutoKill(false).Pause();
    }

    public void ViewBuildingsProtection(RegionController region)
    {
      ClearView();
      CreateIcons(region);
      _animation.Restart();
    }

    private void CreateIcons(RegionController region)
    {
      using (ListPool<IUnit>.Get(out var units))
      using (ListPool<CellController>.Get(out var passedCells))
      {
        foreach (var cell in region.Cells)
        {
          var unit = cell.Unit;
          if (cell.HasUnit && unit.CanViewProtection)
            units.Add(unit);
        }
        
        units.SortByDecreasing(x => x.Protection);

        foreach (var unit in units)
        {
          var cell = unit.Cell;
          var cellPosition = _gridController.HexPositionToWorld(cell.Position);
          unit.GetProtectionArea(GameUtilities.AreaBuffer);
          foreach (var areaCell in GameUtilities.AreaBuffer)
          {
            if (areaCell != cell && !passedCells.Contains(areaCell))
            {
              var instance = _pool.Get();
              var instanceTransform = instance.transform;
              instanceTransform.localScale = Vector3.one;
              _icons.Add(new ProtectionIconData
              {
                Renderer = instance,
                Transform = instanceTransform,
                StartPosition = cellPosition,
                EndPosition = _gridController.HexPositionToWorld(areaCell.Position)
              });
              passedCells.Add(areaCell);
            }
          }
        }
      }
    }

    private void ClearView()
    {
      foreach (var icon in _icons)
        _pool.Release(icon.Renderer);
      _icons.Clear();
      _animation.Pause();
    }
  }
}