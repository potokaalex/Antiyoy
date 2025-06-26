using UnityEngine;

namespace Client.Code.Gameplay.Hex
{
    public static class HexDirectionsUtilities
    {
        private static readonly Vector2Int _south = new(0, -1);
        private static readonly Vector2Int _north = new(0, 1);
        private static readonly Vector2Int _west = new(-1, 0);
        private static readonly Vector2Int _east = new(1, 0);
        private static readonly Vector2Int _southWest = new(-1, -1);
        private static readonly Vector2Int _southEast = new(1, -1);
        private static readonly Vector2Int _northWest = new(-1, 1);
        private static readonly Vector2Int _northEast = new(1, 1);
        private static readonly Vector2Int[] _evenDirections = { _west, _east, _south, _southWest, _north, _northWest };
        private static readonly Vector2Int[] _oddDirections = { _west, _east, _south, _southEast, _north, _northEast };

        public static Vector2Int[] GetNeighborsDirections(Vector2Int hex) => hex.y % 2 == 0 ? _evenDirections : _oddDirections;
    }
}