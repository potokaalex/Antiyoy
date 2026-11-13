using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Client.Project.Hex
{
  [Serializable]
  public struct HexCoordinates : IEquatable<HexCoordinates>
  {
    public int Q;
    public int R;

    private int S => -Q - R;

    public HexCoordinates(int q, int r)
    {
      Q = q;
      R = r;
    }

    public static HexCoordinates FromArray2DIndex(Vector2Int index) => FromArray2DIndex(index.x, index.y);

    public static HexCoordinates FromArray2DIndex(int x, int y) => new(x, y - ((x - (x & 1)) >> 1));

    public Vector2Int ToArray2DIndex()
    {
      var q = Q;
      return new Vector2Int(q, R + ((q - (q & 1)) >> 1));
    }

    public void ToArray2DIndex(out int x, out int y)
    {
      x = Q;
      y = R + ((x - (x & 1)) >> 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int AddOffsetToArray2DIndex(int x, int y, HexCoordinates offset)
    {
      var q = x + offset.Q;
      return new Vector2Int(q, y + offset.R + ((q - (q & 1)) >> 1) - ((x - (x & 1)) >> 1));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int AddOffsetAndConvertToArray2DIndex(HexCoordinates offset)
    {
      var q = Q + offset.Q;
      var r = R + offset.R;
      return new Vector2Int(q, r + ((q - (q & 1)) >> 1));
    }

    public override string ToString() => $"{Q};{R};{S}";

    public static HexCoordinates operator +(HexCoordinates a, HexCoordinates b) => new(a.Q + b.Q, a.R + b.R);

    public static HexCoordinates operator -(HexCoordinates a, HexCoordinates b) => new(a.Q - b.Q, a.R - b.R);

    public static bool operator ==(HexCoordinates a, HexCoordinates b) => a.Equals(b);

    public static bool operator !=(HexCoordinates a, HexCoordinates b) => !a.Equals(b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(HexCoordinates other) => Q == other.Q && R == other.R;

    public override bool Equals(object obj) => obj is HexCoordinates other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Q, R);
  }
}