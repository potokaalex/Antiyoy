using System;
using System.Collections.Generic;
using System.Linq;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
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

    public MapGenerator() => _mapsFactory = Locator.Get<MapsFactory>();

    public MapController Generate(LevelSize levelSize, int colorsCount, string name) => new(GenerateData(levelSize, colorsCount, name));

    public MapData GenerateData(LevelSize levelSize, int colorsCount, string name)
    {
      UnityEngine.Debug.Log(Environment.TickCount);
      var size = GameUtilities.LevelSizeToFieldSize(levelSize);
      var field = _yiotroMapGenerator.GenerateMap(new System.Random(Environment.TickCount), levelSize, colorsCount, size);

      _allPlayerRegions.Shuffle();
      var cells = CreateCells(field, size);
      var regions = _mapsFactory.CreateRegions(cells, size, 10);
      CreateCapitals(regions);
      var players = _mapsFactory.CreatePlayers(regions);

      return _mapsFactory.CreateMapData(players, size, name);
    }

    public MapController GenerateEmpty(LevelSize levelSize, string name) =>
      new(_mapsFactory.CreateMapData(new List<PlayerData>(), GameUtilities.LevelSizeToFieldSize(levelSize), name));

    private CellData?[,] CreateCells(Yiotro.Hex[][] field, Vector2Int size)
    {
      var cells = new CellData?[size.x, size.y];

      for (var x = 0; x < size.x; x++)
      {
        var hexes = field[x];
        for (var y = 0; y < size.y; y++)
        {
          var hex = hexes[y];
          if (hex.Active)
          {
            cells[x, y] = new CellData
            {
              Position = HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)),
              RegionType = GetRegionType(hex),
              Unit = CreateUnitData(hex)
            };
          }
        }
      }

      return cells;
    }

    private UnitData? CreateUnitData(Yiotro.Hex hex)
    {
      if (hex.ObjectInside != 0)
        return new UnitData(YiotroUtilities.ObjToUnitType(hex.ObjectInside), true);
      return null;
    }

    private RegionType GetRegionType(Yiotro.Hex hex)
    {
      var fraction = hex.Fraction;
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
          _mapsFactory.SetUnit(new UnitData(UnitType.Capital), region, cell.Position);
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