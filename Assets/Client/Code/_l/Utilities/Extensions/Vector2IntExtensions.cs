using UnityEngine;

namespace ClientCode.Utilities.Extensions
{
    public static class Vector2IntExtensions
    {
        public static Vector3Int ToVector3Int(this Vector2Int vector) => new(vector.x, vector.y);//TODO: remove

        public static int ToArrayIndex(this Vector2Int index, int arrayWidth) => index.y * arrayWidth + index.x;

        public static Vector2Int ToIndex(this int arrayIndex, int arrayWidth) => new(arrayIndex / arrayWidth, arrayIndex % arrayWidth);

        public static bool InRangeExclusive(this Vector2Int point, Vector2Int range) =>
            point.x >= 0 && point.x <= range.x && point.y > 0 && point.y < range.y;
    }
}