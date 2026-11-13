using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Client.Project
{
  [Serializable]
  public struct UnsafeNullable<T> : IEquatable<UnsafeNullable<T>>
  {
    public T Value;
    public bool HasValue;

    public UnsafeNullable(T value)
    {
      Value = value;
      HasValue = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(UnsafeNullable<T> other)
    {
      var otherHasValue = other.HasValue;
      
      if (!HasValue && !otherHasValue)
        return true;
      
      if (HasValue && otherHasValue)
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
      
      return false;
    }

    public override bool Equals(object obj) => obj is UnsafeNullable<T> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Value, HasValue);
  }
}