using System.Collections.Generic;

namespace Client.Unit.Code
{
  public interface IUnit
  {
    CellController Cell { get; }
    UnitType Type { get; }
    bool HasTurns { get; }
    int Income { get; }
    int CapitalReplacementFactor { get; }
    int Protection { get; }
    bool CanViewProtection { get; }
    bool IsBuilding { get; }
    void ResetTurnsCount();
    void GetMoveArea(List<CellController> outList);
    bool Move(CellController cell);
    void GetProtectionArea(List<CellController> outList);
  }
}