using System;
using System.Collections.Generic;
using System.Linq;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Client.Project.Player;
using Client.Project.Region;
using Client.Project.Unit.Code;
using Client.Project.Utilities;
using Client.Project.Yiotro;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Client.Project.Map
{
  public class MapGenerator
  {
    private readonly YiotroMapGeneratorGeneric _yiotroMapGenerator = new();
    private readonly RegionType[] _allPlayerRegions = GameUtilities.AllPlayerRegions.ToArray();
    private readonly MapsFactory _mapsFactory;
    private UnsafeNullable<CellData>[,] _cells;

    public MapGenerator() => _mapsFactory = Locator.Get<MapsFactory>();

    public MapData GenerateDataAndFirstPlayer(LevelSize levelSize, int regionsCount, string name, int difficultyMode, int playersMode) =>
      GenerateData(levelSize, regionsCount, name, difficultyMode, playersMode, default, true);

    public MapData GenerateData(LevelSize levelSize, int regionsCount, string name, int difficultyMode, int playersMode, PlayerRegionType firstPlayer,
      int id = -1) => GenerateData(levelSize, regionsCount, name, difficultyMode, playersMode, firstPlayer, false, id);

    public MapData GenerateEmptyData(LevelSize levelSize, string name) => _mapsFactory.CreateMapData(new List<PlayerData>(), new List<PlayerData>(),
      levelSize, name, firstPlayer: new PlayerRegionType(RegionType.Lime), playersMode: 1, difficultyMode: 4);

    private MapData GenerateData(LevelSize levelSize, int regionsCount, string name, int difficultyMode, int playersMode,
      PlayerRegionType firstPlayer, bool generateFirstPlayer, int id = -1)
    {
      var size = GameUtilities.LevelSizeToFieldSize(levelSize);
      var field = _yiotroMapGenerator.GenerateMap(new System.Random(Environment.TickCount), levelSize, regionsCount, size);

      _allPlayerRegions.Shuffle();
      _cells = _mapsFactory.CellsBuffer;
      _mapsFactory.ClearCellsBuffer();
      CreateCells(field, size);
      var regions = _mapsFactory.CreateRegions(_cells, size, 10);
      CreateCapitals(regions);

      if (generateFirstPlayer)
      {
        var randomPlayer = Random.Range(0, regionsCount);
        var firstPlayerType = GetRegionType(randomPlayer == YiotroMapGenerator.NEUTRAL_FRACTION ? randomPlayer + 1 : randomPlayer);
        firstPlayer = new PlayerRegionType(firstPlayerType);
      }

      var players = _mapsFactory.CreatePlayers(regions, firstPlayer, playersMode);

      return _mapsFactory.CreateMapData(players, _mapsFactory.CopyPlayers(players), levelSize, name, id: id, difficultyMode: difficultyMode,
        playersMode: playersMode, firstPlayer: firstPlayer);
    }

    private void CreateCells(Yiotro.Hex[,] field, Vector2Int size)
    {
      for (var x = 0; x < size.x; x++)
      for (var y = 0; y < size.y; y++)
      {
        var hex = field[x, y];
        if (hex.Active)
        {
          ref var data = ref _cells[x, y];
          data.HasValue = true;
          data.Value.Position = HexCoordinates.FromArray2DIndex(x, y);
          data.Value.RegionType = GetRegionType(hex.Fraction);
          data.Value.Unit = CreateUnitData(hex);
        }
      }
    }

    private UnsafeNullable<UnitData> CreateUnitData(Yiotro.Hex hex)
    {
      if (hex.ObjectInside != 0)
        return new UnsafeNullable<UnitData>(new UnitData(YiotroUtilities.ObjToUnitType(hex.ObjectInside), true));
      return default;
    }

    private RegionType GetRegionType(int fraction)
    {
      if (fraction == YiotroMapGenerator.NEUTRAL_FRACTION)
        return RegionType.Neutral;
      return _allPlayerRegions[fraction];
    }

    private void CreateCapitals(List<RegionData> regions)
    {
      foreach (var region in regions)
      {
        if (region.Type != RegionType.Neutral)
        {
          var cell = (GetRandomFreeCell(region.Cells) ?? GetRandomCellExceptTowers(region.Cells)) ??
                     region.Cells[Random.Range(0, region.Cells.Count)];
          _mapsFactory.SetUnit(new UnsafeNullable<UnitData>(new UnitData(UnitType.Capital)), region, cell.Position);
        }
      }
    }

    private CellData? GetRandomFreeCell(List<CellData> cells)
    {
      using (ListPool<CellData>.Get(out var resultCells))
      {
        foreach (var cell in cells)
          if (!cell.Unit.HasValue)
            resultCells.Add(cell);
        return resultCells.Count == 0 ? null : resultCells[Random.Range(0, resultCells.Count)];
      }
    }

    private CellData? GetRandomCellExceptTowers(List<CellData> cells)
    {
      using (ListPool<CellData>.Get(out var resultCells))
      {
        foreach (var cell in cells)
          if (!cell.Unit.HasValue || cell.Unit.Value.Type != UnitType.Tower)
            resultCells.Add(cell);
        return resultCells.Count == 0 ? null : resultCells[Random.Range(0, resultCells.Count)];
      }
    }
  }
}