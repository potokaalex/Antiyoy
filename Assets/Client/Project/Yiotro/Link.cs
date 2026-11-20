using System.Runtime.CompilerServices;

namespace Client.Project.Yiotro
{
  public readonly struct Link
  {
    private readonly Hex _p1;
    private readonly Hex _p2;

    public Link(Hex p1, Hex p2)
    {
      _p1 = p1;
      _p2 = p2;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Hex p1, Hex p2) => (_p1 == p1 && _p2 == p2) || (_p1 == p2 && _p2 == p1);
  }
}