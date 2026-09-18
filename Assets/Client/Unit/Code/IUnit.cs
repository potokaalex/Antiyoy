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
    void ResetTurnsCount();
    void DecreaseTurnsCount();
    void GetMoveArea(List<CellController> outList);
    void GetProtectionArea(List<CellController> outList);
  }
}