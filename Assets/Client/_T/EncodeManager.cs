using System;
using System.Linq;
using System.Text;
using Client.Project;
using Client.Project.Government;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Region;
using Client.Project.Unit.Code;
using UnityEngine;

namespace Client._T
{
  public class EncodeManager
  {
    private readonly MapsService _mapsService;
    private readonly GovernmentsService _governmentsService;
    private readonly GridController _gridController;
    private readonly UnitsService _unitsService;
    private readonly RegionsService _regionsService;
    private StringBuilder _builder;

    public EncodeManager()
    {
      _mapsService = Locator.Get<MapsService>();
      _governmentsService = Locator.Get<GovernmentsService>();
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
    }

    public void Encode()
    {
      _builder = new StringBuilder();

      AddWatermark();
      EncodeLevelSize();
      EncodeGeneralInfo();
      EncodeMapName();
      EncodeEditorInfo();
      EncodeLand();
      EncodeUnits();
      EncodeProvinces();
      EncodeRelations();
      EncodeCoalitions();
      EncodeMessages();
      EncodeGoal();
      EncodeRealMoney();

      _builder.Append("#");
      UnityEngine.Debug.Log(_builder.ToString());
    }

    private void EncodeCoalitions()
    {
      StartSection("coalitions");
      _builder.Append("temporary");
      //_builder.append(gameController.levelEditorManager.coalitionsManager.encode());
    }

    private void EncodeRealMoney()
    {
      StartSection("real_money");
      /*for (Province province : fieldManager.provinces)
      {
        Hex capital = province.getCapital();
        _builder.Append(capital.index1)
          .Append(" ")
          .Append(capital.index2)
          .Append(" ")
          .Append(province.money)
          .Append(",");
      }*/
    }

    private void EncodeGoal()
    {
      StartSection("goal");
      _builder.Append("def 0"); //_builder.append(gameController.finishGameManager.encode());
    }

    private void EncodeMessages()
    {
      StartSection("messages");
      //_builder.append(gameController.messagesManager.encode());
    }

    private void EncodeRelations()
    {
      StartSection("relations");
      //_builder.append(gameController.levelEditorManager.editorRelationsManager.encode());
    }

    private void EncodeProvinces()
    {
      StartSection("provinces");
      //_builder.append(gameController.levelEditorManager.editorProvinceManager.encode());

      for (var i = 0; i < _regionsService.Regions.Count; i++)
      {
        var region = _regionsService.Regions[i];
        var firstCellIndex = region.Cells[0].Position.ToArray2DIndex();
        _builder.Append(firstCellIndex.x).Append("@").Append(firstCellIndex.y).Append("@").Append(i).Append("@").Append(i).Append("@")
          .Append(region.Money).Append(",");
      }
    }

    private void EncodeMapName()
    {
      StartSection("map_name");
      _builder.Append(_mapsService.StartMap.Name);
    }

    /*
    private Preferences getPreferences()
    {
      return Gdx.app.getPreferences(EditorSaveSystem.EDITOR_PREFS);
    }
    */

    private void EncodeEditorInfo()
    {
      StartSection("editor_info");
      _builder.Append(1).Append(" "); // _builder.Append(GameRules.editorChosenColor).Append(" "); //? todo
      _builder.Append(false).Append(" ");
      _builder.Append(false).Append(" ");
      _builder.Append(false).Append(" ");
    }

    private void EncodeUnits()
    {
      StartSection("units");
      foreach (var unit in _unitsService.Units)
      {
        if (unit.Type.IsWarrior())
        {
          if (!CanEncodePosition(unit.Cell.Position))
            continue;

          var index = EncodePosition(unit.Cell.Position);
          var strength = unit.Type switch
          {
            UnitType.Peasant => 1,
            UnitType.Spearman => 2,
            UnitType.Infantryman => 3,
            UnitType.Knight => 4,
            _ => 1
          };
          _builder.Append(index.x).Append(" ").Append(index.y).Append(" ").Append(strength).Append(" ").Append(unit.HasTurns).Append(",");
        }
      }
    }

    private void EncodeLand()
    {
      StartSection("land");

      foreach (var cell in _gridController.GetAllCells())
      {
        if (!CanEncodePosition(cell.Position))
          continue;

        var objectInside = 0;
        if (cell.HasUnit)
        {
          objectInside = cell.Unit.Type switch
          {
            UnitType.Farm => 6,
            UnitType.Capital => 3,
            UnitType.Tower => 4,
            UnitType.StrongTower => 7,
            UnitType.Pine => 1,
            UnitType.Palm => 2,
            UnitType.Grave => 5,
            _ => 0
          };
        }

        var fraction = cell.Region.Type switch
        {
          RegionType.Neutral => 7,
          RegionType.Red => 5,
          RegionType.Blue => 2,
          RegionType.Green => 8,
          RegionType.Yellow => 4,
          RegionType.Purple => 9,
          RegionType.Brown => 10,
          RegionType.Cyan => 3,
          RegionType.Lime => 0,
          RegionType.Rose => 1,
          RegionType.Mint => 6,
          _ => throw new ArgumentOutOfRangeException()
        };

        var index = EncodePosition(cell.Position);
        _builder.Append(index.x).Append(" ").Append(index.y).Append(" ").Append(fraction).Append(" ").Append(objectInside).Append(",");
      }
    }

    private void EncodeGeneralInfo()
    {
      //todo
      StartSection("general");
      _builder.Append(1).Append(" "); //_builder.Append(GameRules.difficulty).Append(" ");
      _builder.Append(1).Append(" "); //_builder.Append(gameController.playersNumber).append(" ");
      _builder.Append(_governmentsService.GetAll().Count(x => x.IsAlive)); //_builder.Append(GameRules.fractionsQuantity); или всех непустых?
    }

    private void EncodeLevelSize()
    {
      StartSection("level_size");
      var value = _mapsService.GetLevelSize(_mapsService.StartMap.Size) switch
      {
        LevelSize.Small => 1,
        LevelSize.Medium => 2,
        LevelSize.Big => 4,
        LevelSize.Huge => 9,
        _ => throw new ArgumentOutOfRangeException()
      };
      _builder.Append(value);
    }

    private void AddWatermark() => _builder.Append("antiyoy_level_code");

    private void StartSection(string name) => _builder.Append("#").Append(name).Append(":");

    private Vector2Int EncodePosition(HexCoordinates hex)
    {
      var y = hex.Q;
      var x = hex.R + (hex.Q - (hex.Q & 1)) / 2 - hex.Q / 2;

      x += 26;

      //todo
      //hex.R + hex.Q / 2 - (hex.Q & 1) / 2 - hex.Q / 2
      //hex.R + hex.Q / 2 - hex.Q / 2
      //hex.R

      return new Vector2Int(x, y);
    }

    private bool CanEncodePosition(HexCoordinates cellPosition)
    {
      var index = EncodePosition(cellPosition);
      return index.x >= 0 && index.x <= 84 && index.y >= 0 && index.y <= 54;
    }
  }
}