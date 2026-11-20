using System;
using System.Collections.Generic;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Client.Project.Player;
using Client.Project.Utilities;
using Client.Project.Yiotro;
using UnityEngine;

namespace Client.Project.Map
{
  public class MapDecodeController
  {
    private readonly MapsFactory _mapsFactory;
    private UnsafeNullable<CellData>[,] _cells;
    private Vector2Int _mapSize;
    private LevelSize _levelSize;
    private string _source;
    private string _mapName;
    private bool _isHuge;
    private PlayerRegionType _firstPlayer;
    private int _difficultyMode;
    private int _playersMode;

    public MapDecodeController() => _mapsFactory = Locator.Get<MapsFactory>();

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

    public MapData DecodeToMapData(string levelCode, int id = -1, string name = null)
    {
      _source = levelCode;

      DecodeLevelSize();
      DecodeGeneralInfo();
      DecodeLand();
      DecodeUnits();
      DecodeEditorInfo();
      DecodeMapName(name);

      var regions = _mapsFactory.CreateRegions(_cells, _mapSize, 0);
      DecodeProvinces(regions);

      var players = _mapsFactory.CreatePlayers(regions);
      return _mapsFactory.CreateMapData(players, _mapsFactory.CopyPlayers(players), _levelSize, _mapName, id: id, firstPlayer: _firstPlayer,
        playersMode: _playersMode, difficultyMode: _difficultyMode);
    }

    private void DecodeLevelSize()
    {
      var section = GetSection("level_size");
      if (section == null)
        return;

      _levelSize = YiotroUtilities.CodeToLevelSize(int.Parse(section));
      _mapSize = GameUtilities.LevelSizeToFieldSize(_levelSize);
      _isHuge = _levelSize == LevelSize.Huge;
      _cells = _mapsFactory.CellsBuffer;
      _mapsFactory.ClearCellsBuffer();
    }

    private void DecodeMapName(string name)
    {
      if (name != null)
      {
        _mapName = name;
        return;
      }

      var section = GetSection("map_name");
      if (section != null)
        _mapName = section;
    }

    private void DecodeEditorInfo()
    {
      var section = GetSection("editor_info");
      if (section == null)
        return;

      var split = section.Split(" ");
      _firstPlayer = YiotroUtilities.CodeToPlayerRegionType(int.Parse(split[0]));
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
      _mapsFactory.SetUnit(new UnsafeNullable<UnitData>(new UnitData(unitType, ready)), _cells, index.x, index.y);
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

        var unitData = objectInside != 0 ? new UnsafeNullable<UnitData>(new UnitData(YiotroUtilities.ObjToUnitType(objectInside))) : default;
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
      _difficultyMode = int.Parse(split[0]);
      _playersMode = int.Parse(split[1]);
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