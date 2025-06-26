using UnityEngine;

namespace Client.Code.Gameplay.Hex
{
    public static class HexUtilities
    {
        private static readonly HexCoordinates _south = new(0, 1);
        private static readonly HexCoordinates _southeast = new(1, 0);
        private static readonly HexCoordinates _northeast = new(1, -1);
        private static readonly HexCoordinates _north = new(0, -1);
        private static readonly HexCoordinates _northwest = new(-1, 0);
        private static readonly HexCoordinates _southwest = new(-1, 1);

        public static readonly HexCoordinates[] Directions =
        {
            _south,
            _southeast,
            _northeast,
            _north,
            _northwest,
            _southwest
        };

        public static HexCoordinates FromArrayIndex(Vector2Int index)
        {
            return new HexCoordinates
            {
                Q = index.x,
                R = index.y - (index.x + (index.x & 1)) / 2
            };
        }

        public static Vector2Int ToArrayIndex(this HexCoordinates hex)
        {
            return new Vector2Int
            {
                x = hex.Q,
                y = hex.R + (hex.Q + (hex.Q & 1)) / 2
            };
        }

        public static Vector3 ToWorldPosition(this HexCoordinates hex)
        {
            const float sideLength = 1f / 2;
            const float bigRadius = sideLength;
            const float smallRadius = sideLength * 0.8660254038f; //0.8660254038 = sqrt(3) / 2

            var index = hex.ToArrayIndex();
            var x = index.x * bigRadius * 1.5f;
            var y = index.y * smallRadius * 2;

            y += (index.x & 1) * smallRadius;

            return new Vector2(x, y);
        }
    }
}