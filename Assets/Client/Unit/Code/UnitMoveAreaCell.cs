namespace Client.Unit.Code
{
  public struct UnitMoveAreaCell
  {
    public CellController Cell;
    public int RemainingMove;

    public UnitMoveAreaCell(CellController cell, int remainingMove)
    {
      Cell = cell;
      RemainingMove = remainingMove;
    }
  }
}