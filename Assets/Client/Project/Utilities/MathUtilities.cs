using System.Runtime.CompilerServices;
using UnityEngine;

namespace Client.Project.Utilities
{
  public static class MathUtilities
  {
    public static readonly float HexSide = 0.5f;
    public static readonly float Sqrt3Div2 = Mathf.Sqrt(3f) / 2f;
    public static readonly float HexHeight = HexSide * Sqrt3Div2;
    public static readonly float HexHalfHeight = HexHeight / 2f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToArrayIndex(Vector2Int array2DIndex, int arrayWidth) => ToArrayIndex(array2DIndex.x, array2DIndex.y, arrayWidth);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToArrayIndex(int x, int y, int arrayWidth) => y * arrayWidth + x;
  }
}