using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code.Capital
{
  public class CapitalsMarksController : MonoBehaviour, IInitializable
  {
    [SerializeField] private Transform _prefab;
    [SerializeField] private Vector2 _fromCellCenterOffset;
    private readonly List<Transform> _marks = new();
    private readonly List<IUnit> _units = new();
    private UnitsService _unitsService;
    private GridController _gridController;
    private ObjectPool<Transform> _pool;
    private int _peasantCost;
    private Tween _animation;
    private RegionType? _playerType;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
      _gridController = Locator.Get<GridController>();
      _peasantCost = Locator.Get<UnitsService>().GetCost(UnitType.Peasant);

      _pool = new ObjectPool<Transform>(() => Instantiate(_prefab, transform), t => t.gameObject.SetActive(true), t => t.gameObject.SetActive(false));
      _animation = DOVirtual.Float(0, 1, 0.25f, v =>
      {
        if (!_playerType.HasValue)
          return;

        ClearMarks();

        foreach (var unit in _units)
        {
          if (!RegionCheck(unit.Cell.Region))
            continue;

          var cellCenter = _gridController.HexPositionToWorld(unit.Cell.Position);
          var offset = (Vector3)_fromCellCenterOffset;
          var position = cellCenter + offset;
          position.y += Mathf.Lerp(0, AnimationsUtilities.GameplayUnitsYoyoAnimationOffset, v);
          CreateMark(position);
        }
      }).SetLoops(-1, LoopType.Yoyo).SetId(this).Pause();
    }

    public void Enable()
    {
      _unitsService.OnCreate += AddUnit;
      _unitsService.OnDestroy += DestroyUnit;
    }

    public void Disable()
    {
      _unitsService.OnCreate -= AddUnit;
      _unitsService.OnDestroy -= DestroyUnit;
      _units.Clear();
      ClearPlayerType();
    }

    public void SetPlayerType(RegionType playerType)
    {
      _playerType = playerType;
      _animation.Restart();
    }

    public void ClearPlayerType()
    {
      ClearMarks();
      _playerType = null;
      _animation.Pause();
    }

    private void ClearMarks()
    {
      foreach (var mark in _marks)
        _pool.Release(mark);
      _marks.Clear();
    }

    private void CreateMark(Vector3 position)
    {
      var mark = _pool.Get();
      mark.position = position;
      _marks.Add(mark);
    }

    private void AddUnit(IUnit unit)
    {
      if (unit.Type == UnitType.Capital)
        _units.Add(unit);
    }

    private void DestroyUnit(IUnit unit) => _units.Remove(unit);

    private bool RegionCheck(RegionController region) =>
      region.IsAlive && region.Money >= _peasantCost && _playerType == region.Type;
  }
}