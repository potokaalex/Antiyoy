using System.Collections.Generic;
using UnityEngine;

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
    Vector3 Position { set; }
    void ResetTurns();
    void GetMoveArea(List<CellController> outList);
    void GetProtectionArea(List<CellController> outList);
  }
}