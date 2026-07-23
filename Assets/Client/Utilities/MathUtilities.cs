using UnityEngine;

namespace Client.Utilities
{
  public static class MathUtilities
  {
    public static readonly float HexSide = 0.5f;
    public static readonly float Sqrt3Div2 = Mathf.Sqrt(3f) / 2f;
    public static readonly float HexHeight = HexSide * Sqrt3Div2;
    public static readonly float HexHalfHeight = HexHeight / 2f;

    public static int ToArrayIndex(Vector2Int array2DIndex, int arrayWidth) => array2DIndex.y * arrayWidth + array2DIndex.x;
  }
}