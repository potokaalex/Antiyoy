using UnityEngine;

namespace ClientCode.Utilities.Extensions
{
    public static class Vector2IntExtensions
    {
        public static Vector2Int ToVector2Int(this Vector3Int vector) => new(vector.x, vector.y);
        
        public static Vector3Int ToVector3Int(this Vector2Int vector) => new(vector.x, vector.y);
        
        public static int ToArrayIndex(this Vector2Int index, int arrayWidth) => index.y * arrayWidth + index.x;

        public static Vector2Int ToArrayIndex(this int arrayIndex, int arrayWidth) => new(arrayIndex / arrayWidth, arrayIndex % arrayWidth);
    }
}