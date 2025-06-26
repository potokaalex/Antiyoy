using UnityEngine;

namespace Client.Code.Services.Extensions
{
    public static class Vector2IntExtensions
    {
        public static int ToArrayIndex(this Vector2Int index, int arrayWidth) => index.y * arrayWidth + index.x;

        public static bool InRangeExclusive(this Vector2Int point, Vector2Int range) =>
            point.x >= 0 && point.x < range.x && 
            point.y >= 0 && point.y < range.y;
    }
}