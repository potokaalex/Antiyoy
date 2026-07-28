using System;
using System.Collections.Generic;
using System.Linq;
using Client.Gameplay;
using Client.Infrastructure;
using Client.Region;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code.Capital
{
  public class CapitalsMarkController : MonoBehaviour, IInitializable, ITickable, IDisposable
  {
    [SerializeField] private Transform _prefab;
    [SerializeField] private Vector2 _fromCellCenterOffset;
    [SerializeField] private float _animationOffset;
    private readonly List<RegionController> _regions = new();
    private readonly Dictionary<RegionController, Transform> _marks = new();
    private RegionsService _regionsService;
    private UnitsService _unitsService;
    private CapitalsController _capitalsController;
    private GridController _gridController;
    private GameplayController _gameplayController;
    private int _peasantCost;
    private ObjectPool<Transform> _pool;

    public void Initialize()
    {
      _regionsService = Locator.Get<RegionsService>();
      _capitalsController = Locator.Get<CapitalsController>();
      _gridController = Locator.Get<GridController>();
      _gameplayController = Locator.Get<GameplayController>();
      _peasantCost = Locator.Get<UnitsService>().GetCost(UnitType.Peasant);

      _pool = new ObjectPool<Transform>(() => Instantiate(_prefab, transform), t => t.gameObject.SetActive(true), t => t.gameObject.SetActive(false));
      DOVirtual.Float(0, 1, 0.25f, v =>
      {
        foreach ((var region, var mark) in _marks)
        {
          var cellCenter = _gridController.HexPositionToWorld(_capitalsController.GetCapital(region).Cell.Position);
          var offset = (Vector3)_fromCellCenterOffset;
          var position = cellCenter + offset;
          position.y += Mathf.Lerp(0, _animationOffset, v);
          mark.position = position;
        }
      }).SetLoops(-1, LoopType.Yoyo).SetId(this);
    }

    public void Dispose() => DOTween.Kill(this);

    public void Tick()
    {
      ClearRegions();
      CreateMarks();
    }

    private void ClearRegions()
    {
      for (var i = _regions.Count - 1; i >= 0; i--)
      {
        var region = _regions[i];
        if (!_regionsService.Regions.Contains(region) || !RegionCheck(region))
        {
          _regions.RemoveAt(i);
          _pool.Release(_marks[region]);
          _marks.Remove(region);
        }
      }
    }

    private void CreateMarks()
    {
      foreach (var region in _regionsService.Regions)
      {
        if (RegionCheck(region) && !_regions.Contains(region))
        {
          _regions.Add(region);
          _marks.Add(region, _pool.Get());
        }
      }
    }

    private bool RegionCheck(RegionController region) =>
      region.IsAlive && region.Money >= _peasantCost && _gameplayController.CurrentPlayerRegionType == region.Type;
  }
}