using System.Collections.Generic;
using Client.Infrastructure;
using Client.Region;
using DG.Tweening;
using UnityEngine;

namespace Client.Unit.Code
{
  public class WarriorUnitsAnimator : IInitializable
  {
    private readonly List<IUnit> _units = new();
    private UnitsService _unitsService;
    private GridController _gridController;
    private RegionType? _playerType;
    private Tween _animation;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
      _gridController = Locator.Get<GridController>();
      _animation = DOVirtual.Float(0, 1, 0.25f, v =>
      {
        if (!_playerType.HasValue)
          return;

        foreach (var unit in _units)
        {
          if (unit.Cell.Region.Type != _playerType.Value)
            continue;

          if (!unit.HasTurns)
          {
            SetUnitCellCenterPosition(unit);
            continue;
          }

          var position = _gridController.HexPositionToWorld(unit.Cell.Position);
          position.y += Mathf.Lerp(0, 0.05f, v);
          unit.Position = position;
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
      _animation.Restart();
      _playerType = playerType;
    }

    public void ClearPlayerType()
    {
      foreach (var unit in _units)
        SetUnitCellCenterPosition(unit);
      _playerType = null;
      _animation.Pause();
    }

    private void AddUnit(IUnit unit)
    {
      if (unit.Type.IsWarrior())
        _units.Add(unit);
    }

    private void DestroyUnit(IUnit unit) => _units.Remove(unit);

    private void SetUnitCellCenterPosition(IUnit unit) => unit.Position = _gridController.HexPositionToWorld(unit.Cell.Position);
  }
}