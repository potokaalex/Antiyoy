using System;
using System.Collections.Generic;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Client.Project.Utilities;
using Client.Project.Yiotro;
using UnityEngine;

namespace Client.Project.Map
{
  public class MapDecodeController : IInitializable
  {
    private MapsFactory _mapsFactory;
    private Vector2Int _mapSize;
    private string _source;
    private UnsafeNullable<CellData>[,] _cells;
    private string _mapName;
    private bool _isHuge;

    public void Initialize() => _mapsFactory = Locator.Get<MapsFactory>();

    public bool IsValidLevelCode(string levelCode)
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

    public MapData DecodeToMapData(string levelCode)
    {
      _source = levelCode;

      DecodeLevelSize();
      DecodeGeneralInfo();
      DecodeLand();
      DecodeUnits();
      DecodeEditorInfo();
      DecodeMapName();

      var regions = _mapsFactory.CreateRegions(_cells, _mapSize, 0);
      DecodeProvinces(regions);
      var players = _mapsFactory.CreatePlayers(regions);
      return _mapsFactory.CreateMapData(players, _mapSize, _mapName);
    }

    private void DecodeLevelSize()
    {
      var section = GetSection("level_size");
      if (section == null)
        return;

      var levelSize = YiotroUtilities.CodeToLevelSize(int.Parse(section));

      _mapSize = GameUtilities.LevelSizeToFieldSize(levelSize);
      _isHuge = levelSize == LevelSize.Huge;
      _cells = _mapsFactory.CellsBuffer;
      _mapsFactory.ClearCellsBuffer(_mapSize);
    }

    private void DecodeMapName()
    {
      var section = GetSection("map_name");
      if (section == null)
        return;

      //todo: применить на mapData, чтобы слот загруженной карты назывался по-новому. При загрузке в редакторе - игнорировать.
      _mapName = section;
    }

    private void DecodeEditorInfo()
    {
      var section = GetSection("editor_info");
      if (section == null)
        return;

      var split = section.Split(" ");

      //todo
      //GameRules.setEditorChosenColor(Integer.valueOf(split[0]));
    }

    private void DecodeUnits()
    {
      var section = GetSection("units");
      if (section == null)
        return;

      foreach (var token in section.Split(",", StringSplitOptions.RemoveEmptyEntries))
        EncodeSingleUnit(token);
    }

    private void EncodeSingleUnit(string token)
    {
      var split = token.Split(" ");
      var index1 = int.Parse(split[0]);
      var index2 = int.Parse(split[1]);
      var strength = int.Parse(split[2]);
      var ready = bool.Parse(split[3]);
      var unitType = YiotroUtilities.StrengthToUnitType(strength);
      var index = YiotroUtilities.IndexToHex(index1, index2, _isHuge).ToArray2DIndex();
      _mapsFactory.SetUnit(new UnitData(unitType, ready), _cells, index.x, index.y);
    }

    private void DecodeLand()
    {
      var section = GetSection("land");
      if (section == null)
        return;

      DecodeLand(section);
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
        var hex = YiotroUtilities.IndexToHex(index1, index2, _isHuge);
        var index = hex.ToArray2DIndex();

        UnitData? unitData = null;
        if (objectInside != 0)
        {
          var unitType = YiotroUtilities.ObjToUnitType(objectInside);
          unitData = new UnitData(unitType);
        }

        var regionType = YiotroUtilities.FractionToRegionType(fraction);

        ref var cell = ref _cells[index.x, index.y];
        cell.HasValue = true;
        cell.Value.Position = hex;
        cell.Value.RegionType = regionType;
        cell.Value.Unit = unitData;
      }
    }

    private void DecodeGeneralInfo()
    {
      var section = GetSection("general");
      if (section == null)
        return;

      var split = section.Split(" ");
      //todo
      //GameRules.setDifficulty(int.Parse(split[0]));
      //gameController.setPlayersNumber(int.Parse(split[1]));
      //GameRules.setFractionsQuantity(int.Parse(split[2]));
    }

    private void DecodeProvinces(List<RegionData> regions)
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
        var hex = YiotroUtilities.IndexToHex(index1, index2, _isHuge);
        _mapsFactory.GetRegionByCell(hex, regions).Money = money;
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
  }
}