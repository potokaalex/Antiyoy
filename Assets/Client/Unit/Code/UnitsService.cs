using System.Collections.Generic;
using System.Linq;
using Client.Hex;
using Client.Infrastructure;
using Client.Region;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Unit.Code
{
  public class UnitsService : SerializedMonoBehaviour, IInitializable
  {
    [SerializeField] private UnitController _unitPrefab;
    [SerializeField] private Dictionary<UnitType, UnitConfig> _unitsConfigs;
    private readonly List<IUnit> _units = new();
    private UnitsAreaCalculator _areaCalculator;
    private GridController _gridController;
    private ObjectPool<UnitController> _pool;

    public void Initialize()
    {
      _areaCalculator = Locator.Get<UnitsAreaCalculator>();
      _gridController = Locator.Get<GridController>();
      _pool = new(() => Instantiate(_unitPrefab, transform), x => x.gameObject.SetActive(true),
        x => x.gameObject.SetActive(false));
    }

    public void InitialCreateUnits()
    {
      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(0, 0)), out var redCapitalCell);
      CreateUnit(redCapitalCell, UnitType.Capital, true);

      _gridController.GetCell(HexCoordinates.FromArray2DIndex(new Vector2Int(8, 0)), out var blueCapitalCell);
      CreateUnit(blueCapitalCell, UnitType.Capital, true);
    }

    public void Clear()
    {
      for (var i = _units.Count - 1; i >= 0; i--)
        Destroy(_units[i]);
    }

    public void Create(CellController cell, UnitType type, bool hasTurns = true) => CreateUnit(cell, type, hasTurns);

    public void Destroy(IUnit unit)
    {
      if (unit != null)
      {
        var unitController = (UnitController)unit;
        unitController.Dispose();
        _units.Remove(unit);
        _pool.Release(unitController);
      }
    }

    public bool Get(CellController cell, out IUnit unit)
    {
      unit = cell.Unit;
      return unit != null;
    }

    public void GetUnitCreationArea(RegionController region, List<CellController> outResult, UnitType unitType) =>
      _areaCalculator.GetCreationArea(region, outResult, unitType);

    public int GetCost(UnitType type)
    {
      var creationCost = _unitsConfigs[type].CreationCost;
      if (type == UnitType.Farm)
        return creationCost + _units.Count(x => x.Type == UnitType.Farm) * 2;
      return creationCost;
    }

    public Sprite GetSprite(UnitType unitType) => _unitsConfigs[unitType].Sprite;

    private void CreateUnit(CellController cell, UnitType type, bool hasTurns)
    {
      Destroy(cell.Unit);
      var instance = _pool.Get();
      instance.Initialize(_unitsConfigs[type], cell, hasTurns);
      _units.Add(instance);
    }

    public bool CanMove(IUnit unit, CellController cell) => CanCreate(cell, unit.Cell.Region.Type) && unit.HasTurns;

    public bool CanCreate(CellController cell, RegionType playerRegion) => !(cell.Region.Type == playerRegion && cell.HasUnit);
  }
}