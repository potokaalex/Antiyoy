using UnityEngine;

namespace Code.Hex
{
    public static class HexUtilities
    {
        private static readonly HexCoordinates South = new(0, 1);
        private static readonly HexCoordinates Southeast = new(1, 0);
        private static readonly HexCoordinates Northeast = new(1, -1);
        private static readonly HexCoordinates North = new(0, -1);
        private static readonly HexCoordinates Northwest = new(-1, 0);
        private static readonly HexCoordinates Southwest = new(-1, 1);
        
        public static readonly HexCoordinates[] Directions =
        {
            South,
            Southeast,
            Northeast,
            North,
            Northwest,
            Southwest
        };
        
        public static HexCoordinates FromArrayIndex(Vector2Int index)
        {
            return new HexCoordinates
            {
                Q = index.x,
                R = index.y - (index.x - (index.x & 1)) / 2
            };
        }

        public static Vector2Int ToArrayIndex(this HexCoordinates hex)
        {
            return new Vector2Int
            {
                x = hex.Q,
                y = hex.R + (hex.Q - (hex.Q & 1)) / 2
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