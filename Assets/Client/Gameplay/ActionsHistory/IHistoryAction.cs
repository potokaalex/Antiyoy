namespace Client.Gameplay.ActionsHistory
{
  public interface IHistoryAction
  {
    void Undo();

    void Dispose()
    {
    }
  }
}