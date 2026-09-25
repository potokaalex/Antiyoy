using UnityEngine;

namespace Client.Unit.Code
{
  [CreateAssetMenu(menuName = "Client/Configs/Unit", fileName = "UnitConfig", order = 0)]
  public class UnitConfig : ScriptableObject
  {
    public UnitType Type;
    public UnitController Prefab;
    public Sprite Sprite;
    public int Income;
    public int CreationCost;
    public bool HasTurns;
    public int CapitalReplacementFactor;
    public int Protection;
    public int Attack;
    public int JoinFactor;
  }
}