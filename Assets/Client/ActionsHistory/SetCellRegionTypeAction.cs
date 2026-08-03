using Client.Region;

namespace Client.ActionsHistory
{
  public class SetCellRegionTypeAction : IHistoryAction
  {
    private readonly CellController _cell;
    private readonly RegionType _oldRegionType;

    public SetCellRegionTypeAction(CellController cell, RegionType oldRegionType)
    {
      _cell = cell;
      _oldRegionType = oldRegionType;
    }

    //скажем, ну, базовое - захват клетки. Клетка была за x регион-типом. Мы устанавливаем нового владельца региона.
    public void Undo()
    {
      //setRegionType(_cell, _oldRegionType)
      //а сервис откуда взять?
      //впринципе, все возможные действия с игровым полем лучше сделать через 1 сервис.
    }
  }
}