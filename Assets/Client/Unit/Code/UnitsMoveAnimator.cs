using Client.Infrastructure;
using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class UnitsMoveAnimator : MonoBehaviour, IInitializable
  {
    [SerializeField] private SpriteRenderer _movementPrefab;
    private GridController _gridController;
    private UnitsService _unitsService;
    private ObjectPool<SpriteRenderer> _pool;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _pool = new ObjectPool<SpriteRenderer>(() => Instantiate(_movementPrefab, transform), v => v.enabled = true, v => v.enabled = false);
    }

    public void PlayMove(CellController fromCell, CellController toCell, UnitType unitType)
    {
      var startPosition = _gridController.HexPositionToWorld(fromCell.Position);
      var instance = Create(startPosition, unitType);
      var toCellUnit = toCell.Unit;
      var unitTypeChanged = toCell.Unit.Type != unitType;

      if (!unitTypeChanged)
        toCellUnit.SetActiveRenderer(false);

      DOVirtual.Float(0, 1, 0.1f, v =>
      {
        instance.transform.position = Vector3.Lerp(startPosition, toCellUnit.Position, v);
        if (unitTypeChanged) 
          instance.SetAlpha(1 - v);
      }).OnComplete(() =>
      {
        Destroy(instance);
        toCellUnit.SetActiveRenderer(true);
      });
    }

    private SpriteRenderer Create(Vector3 position, UnitType unitType)
    {
      var instance = _pool.Get();
      instance.transform.position = position;
      instance.sprite = _unitsService.GetSprite(unitType);
      instance.SetAlpha(1);
      return instance;
    }

    private void Destroy(SpriteRenderer instance) => _pool.Release(instance);
  }
}