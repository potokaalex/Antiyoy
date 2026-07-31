using System.Collections.Generic;
using Client.Region;
using Client.Unit.Code;

namespace Client._Back
{
  public class GameFieldController
  {
    //все возможные операции на игровом поле здесь. зачем? Чтобы понять что вообще возможно делать и отменять, чисто временно?
    public void SetRegionType(CellController cell, RegionType regionType)
    {
      //ок. типа захват клетки.
      //А ЧТО НАСЧЁТ ДЕНЕГ В РЕГИОНЕ, И ДАННЫХ РЕГИОНА(типа имени)? - ну, это можно в отдельное внутренние действие записать.
    }

    //создание и удаление клетки невозможно во время игры.
    public void CreateUnit(CellController cell, UnitType unitType) //юнит не должен знать к кому он принадлежит, его принадлежность это клетка на которой он стоит.
    {
    }

    public void DestroyUnit(CellController cell)
    {
    }
    
    //ну, вот и всё, это всё, что может сделать игрок, лол. 
  }

  public class SetCellRegionTypeOperation : IBackOperation
  {
    private readonly CellController _cell;
    private readonly RegionType _oldRegionType;

    public SetCellRegionTypeOperation(CellController cell, RegionType oldRegionType)
    {
      _cell = cell;
      _oldRegionType = oldRegionType;
    }

    //скажем, ну, базовое - захват клетки. Клетка была за x регион-типом. Мы устанавливаем нового владельца региона.
    public void Reset()
    {
      //setRegionType(_cell, _oldRegionType)
      //а сервис откуда взять?
      //впринципе, все возможные действия с игровым полем лучше сделать через 1 сервис.
    }
  }

  public interface IBackOperation
  {
    void Reset();
  }

  public class BackController//history controller? Ну, задний контроллер - звучит неоч.
  {
    private List<IBackOperation> _operations; //каждая операция будет создавать новый объект, нехорошо.

    public void Back()
    {
      //откатить историю назад.
      //каждое действие на карте - определённая операция.
      //каждая операция имеет какой-то откат.
    }
  }
}