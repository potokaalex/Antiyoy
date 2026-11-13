using System;
using System.Collections.Generic;

namespace Client.Project
{
  public struct UnsafeNullable<T> : IEquatable<UnsafeNullable<T>>
  {
    public T Value;
    public bool HasValue;

    private UnsafeNullable(T value)
    {
      Value = value;
      HasValue = true;
    }

    public static implicit operator UnsafeNullable<T>(T value) => new(value);

    public bool Equals(UnsafeNullable<T> other) => HasValue == other.HasValue && EqualityComparer<T>.Default.Equals(Value, other.Value);

    public override bool Equals(object obj) => obj is UnsafeNullable<T> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Value, HasValue);
  }
}