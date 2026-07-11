namespace Client.Unit.Code
{
  public static class UnitsHelper
  {
    public static bool IsBuilding(this UnitType type) => type is UnitType.Capital or UnitType.Farm or UnitType.Tower;
  }
}