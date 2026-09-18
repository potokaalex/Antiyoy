namespace Client.Unit.Code
{
  public static class UnitsHelper
  {
    public static bool IsBuilding(this UnitType type) => type is UnitType.Capital or UnitType.Farm or UnitType.Tower;
    public static bool IsWarrior(this UnitType type) => type is UnitType.Peasant or UnitType.Spearman or UnitType.Infantryman or UnitType.Knight;
  }
}