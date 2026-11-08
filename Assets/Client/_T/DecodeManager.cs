using System;
using System.Collections.Generic;
using System.Linq;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Map.Data;
using Client.Project.Region;
using Client.Project.Unit.Code;

namespace Client._T
{
  public class DecodeManager
  {
    private readonly MapsService _mapsService;
    private string _source;
    private MapData _map;
    private bool _isHuge;
    private CellData?[,] _cells;

    public DecodeManager()
    {
      _mapsService = Locator.Get<MapsService>();
    }

    protected bool IsValidLevelCode(string levelCode)
    {
      if (levelCode == null)
        return false;
      if (!levelCode.Contains("antiyoy_level_code"))
        return false;
      if (!levelCode.Contains("level_size"))
        return false;
      if (!levelCode.Contains("units"))
        return false;
      if (!levelCode.Contains("land"))
        return false;
      return true;
    }

    public MapData Perform(string levelCode)
    {
      _source = levelCode;

      _map = new MapData();

      ApplyLevelSize();
      ApplyGeneralInfo();
      ApplyLand();
      ApplyUnits();
      ApplyEditorInfo();
      ApplyMapName();

      //выделить фрацкии, а, подожди, ещё распарсить деньги, тут нужна логика, которая преобразует поле в карту, такую логику я писал в генераторе.

      var generator = new MapGenerator();
      var regions = generator.CreateRegions(_cells, _map.Size, 0);
      ApplyProvinces(regions);
      var players = generator.CreatePlayers(regions);

      _map.InitialPlayers = players;
      _map.Players = players.Select(x => x.Copy()).ToList(); //повторяется в генераторе.
      return _map;
    }

    private void ApplyLevelSize()
    {
      var section = GetSection("level_size");
      if (section == null)
        return;

      var levelSize = int.Parse(section) switch
      {
        1 => LevelSize.Small,
        2 => LevelSize.Medium,
        4 => LevelSize.Big,
        9 => LevelSize.Huge,
        _ => throw new ArgumentOutOfRangeException()
      };

      _map.Size = _mapsService.GetFieldSize(levelSize);
      _isHuge = levelSize == LevelSize.Huge;
      _cells = new CellData?[_map.Size.x, _map.Size.y];
    }

    private void ApplyMapName()
    {
      var mapNameSection = GetSection("map_name");
      if (mapNameSection == null)
        return;

      //todo: применить на mapData, чтобы слот загруженной карты назывался по-новому. При загрузке в редакторе - игнорировать.
      _map.Name = mapNameSection;
    }

    private void ApplyEditorInfo()
    {
      var editorInfoSection = GetSection("editor_info");
      if (editorInfoSection == null)
        return;

      var split = editorInfoSection.Split(" ");

      //todo
      //GameRules.setEditorChosenColor(Integer.valueOf(split[0]));
    }

    private void ApplyUnits()
    {
      var unitsSection = GetSection("units");
      if (unitsSection == null)
        return;

      foreach (var token in unitsSection.Split(",", StringSplitOptions.RemoveEmptyEntries))
        EncodeSingleUnit(token);
    }

    private void EncodeSingleUnit(string token)
    {
      var split = token.Split(" ");
      var index1 = int.Parse(split[0]);
      var index2 = int.Parse(split[1]);
      var strength = int.Parse(split[2]);
      var ready = bool.Parse(split[3]);

      var unitType = strength switch
      {
        1 => UnitType.Peasant,
        2 => UnitType.Spearman,
        3 => UnitType.Infantryman,
        4 => UnitType.Knight,
        _ => throw new ArgumentOutOfRangeException()
      };

      var index = DecodePosition(index1, index2).ToArray2DIndex();
      var cell = _cells[index.x, index.y];

      if (cell.HasValue)
      {
        var c = cell.Value;
        c.Unit = new UnitData
        {
          Type = unitType,
          HasTurns = ready
        };
        _cells[index.x, index.y] = c;
      }
    }

    private void ApplyLand()
    {
      var landSection = GetSection("land");
      if (landSection == null)
        return;

      DecodeLand(landSection);
    }

    private void DecodeLand(string source)
    {
      foreach (var token in source.Split(",", StringSplitOptions.RemoveEmptyEntries))
      {
        var split = token.Split(" ");
        var index1 = int.Parse(split[0]);
        var index2 = int.Parse(split[1]);
        var fraction = int.Parse(split[2]);
        var objectInside = int.Parse(split[3]);
        var hex = DecodePosition(index1, index2);
        var index = hex.ToArray2DIndex();

        UnitData? unitData = null;
        if (objectInside != 0)
        {
          var unitType = objectInside switch
          {
            6 => UnitType.Farm,
            3 => UnitType.Capital,
            4 => UnitType.Tower,
            7 => UnitType.StrongTower,
            1 => UnitType.Pine,
            2 => UnitType.Palm,
            5 => UnitType.Grave,
            _ => throw new ArgumentOutOfRangeException()
          };

          unitData = new UnitData
          {
            Type = unitType,
            HasTurns = false
          };
        }

        var regionType = fraction switch
        {
          7 => RegionType.Neutral,
          5 => RegionType.Red,
          2 => RegionType.Blue,
          8 => RegionType.Green,
          4 => RegionType.Yellow,
          9 => RegionType.Purple,
          10 => RegionType.Brown,
          3 => RegionType.Cyan,
          0 => RegionType.Lime,
          1 => RegionType.Rose,
          6 => RegionType.Mint,
          _ => throw new ArgumentOutOfRangeException()
        };

        _cells[index.x, index.y] = new CellData
        {
          Position = hex,
          RegionType = regionType,
          Unit = unitData
        };
      }
    }

    private void ApplyGeneralInfo()
    {
      var generalSection = GetSection("general");
      if (generalSection == null)
        return;

      var split = generalSection.Split(" ");
      //todo
      //GameRules.setDifficulty(int.Parse(split[0]));
      //gameController.setPlayersNumber(int.Parse(split[1]));
      //GameRules.setFractionsQuantity(int.Parse(split[2]));
    }

    private void ApplyProvinces(List<RegionData> regions)
    {
      var section = GetSection("provinces");
      if (section == null)
        return;

      foreach (var token in section.Split(",", StringSplitOptions.RemoveEmptyEntries))
      {
        var split = token.Split("@");
        var index1 = int.Parse(split[0]);
        var index2 = int.Parse(split[1]);
        var money = int.Parse(split[4]);
        var hex = DecodePosition(index1, index2);

        for (var i = 0; i < regions.Count; i++)
        {
          var region = regions[i];
          if (region.Cells.Any(x => x.Position == hex))
          {
            var r = region;
            r.Money = money;
            regions[i] = r;
            break;
          }
        }
      }
    }

    private string GetSection(string name)
    {
      var nameIndex = _source.IndexOf("#" + name, StringComparison.Ordinal);
      if (nameIndex == -1)
        return null;
      nameIndex++;

      var colonIndex = _source.IndexOf(":", nameIndex, StringComparison.Ordinal);
      var hashIndex = _source.IndexOf("#", colonIndex, StringComparison.Ordinal);
      if (hashIndex - colonIndex < 2)
        return null;

      return _source.Substring(colonIndex + 1, hashIndex - colonIndex - 1);
    }

    private HexCoordinates DecodePosition(int x, int y)
    {
      return new HexCoordinates
      {
        R = x - (_isHuge ? 20 : 26),
        Q = y
      };
    }
  }
}