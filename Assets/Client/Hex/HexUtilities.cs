namespace Client.Hex
{
  public static class HexUtilities
  {
    public static readonly HexCoordinates North = new(0, 1);
    public static readonly HexCoordinates Northeast = new(1, 0);
    public static readonly HexCoordinates Southeast = new(1, -1);
    public static readonly HexCoordinates South = new(0, -1);
    public static readonly HexCoordinates Southwest = new(-1, 0);
    public static readonly HexCoordinates Northwest = new(-1, 1);
    public static readonly HexCoordinates[] Directions = { North, Northeast, Southeast, South, Southwest, Northwest };
  }
}