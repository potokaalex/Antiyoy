using UnityEngine;

namespace Client
{
  public static class MathUtilities
  {
    public static int ToArrayIndex(Vector2Int array2DIndex, int arrayWidth) => array2DIndex.y * arrayWidth + array2DIndex.x;
  }
}