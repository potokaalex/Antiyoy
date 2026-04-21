namespace Client.Hex
{
  public static class HexUtilities
  {
    private static readonly HexCoordinates _north = new(0, -1);
    private static readonly HexCoordinates _northeast = new(1, -1);
    private static readonly HexCoordinates _southeast = new(1, 0);
    private static readonly HexCoordinates _south = new(0, 1);
    private static readonly HexCoordinates _southwest = new(-1, 1);
    private static readonly HexCoordinates _northwest = new(-1, 0);

    public static readonly HexCoordinates[] Directions = { _north, _northeast, _southeast, _south, _southwest, _northwest };
  }
}