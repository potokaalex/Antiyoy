namespace Client.Unit.Code
{
  public struct UnitMoveAreaCell
  {
    public readonly CellController Cell;
    public readonly int RemainingMove;

    public UnitMoveAreaCell(CellController cell, int remainingMove)
    {
      Cell = cell;
      RemainingMove = remainingMove;
    }
  }
}