using System;
using System.Collections.Generic;
using Client.Project.Hex;
using Client.Project.Map.Data;
using Client.Project.Map.Yiotro;
using Client.Project.Region;
using Client.Project.Unit.Code;
using Client.Project.Utilities;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Client.Project.Map
{
  public class MapGenerator
  {
    private readonly RegionType[] _allPlayerRegions =
    {
      RegionType.Red, RegionType.Blue, RegionType.Green, RegionType.Yellow, RegionType.Purple, RegionType.Brown, RegionType.Cyan, RegionType.Lime,
      RegionType.Rose, RegionType.Mint
    };

    private readonly YiotroMapGeneratorGeneric _yiotroMapGenerator = new();
    private readonly HashSet<CellData> _passedBuffer = new();
    private readonly Queue<CellData> _frontBuffer = new();

    public MapController Generate(LevelSize levelSize, int colorsCount)
    {
      var field = _yiotroMapGenerator.GenerateMap(new System.Random(Environment.TickCount), levelSize, colorsCount);

      _allPlayerRegions.Shuffle();
      var size = new Vector2Int(field.Length, field[0].Length);
      var cells = CreateCells(field, size);
      var regions = CreateRegions(cells, size);
      CreateCapitals(regions);

      return new MapController(new MapData
      {
        Players = CreatePlayers(regions),
        Size = size,
        Name = "Skirmish"
      });
    }

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
      if (hex.ObjectInside == 0)
        return null;

      return new UnitData
      {
        Type = GetObject(hex),
        HasTurns = true
      };
    }

    private UnitType GetObject(Yiotro.Hex hex)
    {
      switch (hex.ObjectInside)
      {
        case YiotroMapGenerator.OBJ_PINE:
          return UnitType.Pine;
        case YiotroMapGenerator.OBJ_PALM:
          return UnitType.Palm;
        default:
          throw new Exception($"Cant GetObject for: {hex.ObjectInside}");
      }
    }

    private RegionType GetRegionType(Yiotro.Hex hex)
    {
      var fraction = hex.Fraction;
      if (fraction == YiotroMapGenerator.NEUTRAL_FRACTION)
        return RegionType.Neutral;
      return _allPlayerRegions[fraction];
    }

    private List<RegionData> CreateRegions(CellData?[,] cells, Vector2Int size)
    {
      var newRegions = new List<RegionData>();
      _passedBuffer.Clear();
      _frontBuffer.Clear();

      foreach (var rootCellNull in cells)
      {
        if (!rootCellNull.HasValue)
          continue;

        var rootCell = rootCellNull.Value;

        if (_passedBuffer.Contains(rootCell))
          continue;

        var newRegion = new RegionData
        {
          Cells = new List<CellData>(),
          Type = rootCell.RegionType,
          Money = 10
        };
        newRegions.Add(newRegion);

        _frontBuffer.Enqueue(rootCell);
        _passedBuffer.Add(rootCell);
        newRegion.Cells.Add(rootCell);

        while (_frontBuffer.Count > 0)
        {
          var cell = _frontBuffer.Dequeue();

          foreach (var neighbour in GetNeighbourCells(cells, cell, size))
          {
            if (neighbour.RegionType == rootCell.RegionType && !_passedBuffer.Contains(neighbour))
            {
              newRegion.Cells.Add(neighbour);
              _frontBuffer.Enqueue(neighbour);
              _passedBuffer.Add(neighbour);
            }
          }
        }
      }

      return newRegions;
    }

    private IEnumerable<CellData> GetNeighbourCells(CellData?[,] cells, CellData cell, Vector2Int size)
    {
      var aroundPosition = cell.Position;

      foreach (var direction in HexUtilities.Directions)
      {
        var index = (aroundPosition + direction).ToArray2DIndex();
        if (index.x >= 0 && index.x < size.x && index.y >= 0 && index.y < size.y)
        {
          var item = cells[index.x, index.y];
          if (item.HasValue)
            yield return item.Value;
        }
      }
    }

    private List<PlayerData> CreatePlayers(List<RegionData> regions)
    {
      var players = new List<PlayerData>();

      while (regions.Count > 0)
      {
        var rootRegion = regions[0];
        regions.RemoveAt(0);

        var player = new PlayerData
        {
          Regions = new()
        };

        players.Add(player);
        player.Regions.Add(rootRegion);

        for (var i = regions.Count - 1; i >= 0; i--)
        {
          var region = regions[i];
          if (region.Type == rootRegion.Type)
          {
            player.Regions.Add(region);
            regions.RemoveAt(i);
          }
        }
      }

      return players;
    }

    private void CreateCapitals(List<RegionData> regions)
    {
      foreach (var region in regions)
      {
        if (region.Type != RegionType.Neutral)
        {
          var cell = (GetRandomFreeCell(region.Cells) ?? GetRandomCellExceptTowers(region.Cells)) ??
                     region.Cells[Random.Range(0, region.Cells.Count)];

          cell.Unit = new UnitData
          {
            Type = UnitType.Capital,
            HasTurns = false
          };

          for (var i = 0; i < region.Cells.Count; i++)
          {
            if (region.Cells[i].Position == cell.Position)
            {
              region.Cells[i] = cell;
              break;
            }
          }
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