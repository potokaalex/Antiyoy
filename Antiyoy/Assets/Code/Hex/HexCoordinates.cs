using System;

namespace Code.Hex
{
    public struct HexCoordinates : IEquatable<HexCoordinates>
    {
        public int Q;
        public int R;

        public int S => -Q - R;

        public HexCoordinates(int q, int r)
        {
            Q = q;
            R = r;
        }

        public override string ToString() => $"{Q};{R};{S}";
        
        public static HexCoordinates operator +(HexCoordinates a, HexCoordinates b)
        {
            a.Q += b.Q;
            a.R += b.R;

            return a;
        }

        public static bool operator ==(HexCoordinates a, HexCoordinates b) => a.Equals(b);

        public static bool operator !=(HexCoordinates a, HexCoordinates b) => !a.Equals(b);

        public bool Equals(HexCoordinates other) => Q == other.Q && R == other.R;

        public override bool Equals(object obj) => obj is HexCoordinates other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Q, R);
    }
}