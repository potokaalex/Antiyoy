using System;
using System.Collections.Generic;
using Antiyoy.Gameplay;
using Client.Project.Hex;
using Client.Project.Map;
using Client.Project.Region;
using Client.Project.Unit.Code;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Client._T
{
  public class MapGeneratorT
  {
    public MapPureData Generate(LevelSize levelSize, int playersCount)
    {
      var map = new MapPureData();
      map.Size = GetMapSize(levelSize);

      UnityEngine.Debug.Log(playersCount);
      GameRules.fractionsQuantity = playersCount;
      var gameController = new GameController(map.Size.x, map.Size.y, levelSize);
      var mapGeneratorGeneric = new MapGeneratorGeneric(gameController);
      gameController.fieldManager.createFieldMatrix();
      mapGeneratorGeneric.generateMap(new System.Random(Environment.TickCount), gameController.fieldManager.field);

      var cells = CreateCells(gameController.fieldManager.field);
      var regions = CreateRegions(cells);
      CreateCapitals(regions);
      map.Players = CreatePlayers(regions);

      return map;
    }

    private Vector2Int GetMapSize(LevelSize levelSize)
    {
      switch (levelSize)
      {
        case LevelSize.Small:
          return new Vector2Int(14, 24);
        case LevelSize.Medium:
          return new Vector2Int(28, 24);
        case LevelSize.Big:
          return new Vector2Int(28, 48);
        case LevelSize.Huge:
          return new Vector2Int(42, 72);
        default:
          throw new ArgumentOutOfRangeException(nameof(levelSize), levelSize, null);
      }
    }

    private CellPureData?[,] CreateCells(Hex[][] field)
    {
      var cells = new CellPureData?[field.Length, field[0].Length];

      for (var x = 0; x < field.Length; x++)
      {
        var hexes = field[x];
        for (var y = 0; y < hexes.Length; y++)
        {
          var hex = hexes[y];
          if (hex.active)
          {
            cells[x, y] = new CellPureData
            {
              Position = HexCoordinates.FromArray2DIndex(new Vector2Int(x, y)),
              RegionType = GetFraction(hex),
              Unit = CreateUnitData(hex)
            };
          }
        }
      }

      return cells;
    }

    private UnitPureData? CreateUnitData(Hex hex)
    {
      if (hex.objectInside == 0)
        return null;

      return new UnitPureData
      {
        Type = GetObject(hex),
        HasTurns = true
      };
    }

    private UnitType GetObject(Hex hex)
    {
      switch (hex.objectInside)
      {
        case Obj.PINE:
          return UnitType.Pine;
        case Obj.PALM:
          return UnitType.Palm;
        default:
          throw new Exception($"Cant GetObject for: {hex.objectInside}");
      }
    }

    private RegionType GetFraction(Hex hex)
    {
      switch (hex.fraction)
      {
        case 7:
          return RegionType.Neutral;
        case 1:
          return RegionType.Red;
        case 2:
          return RegionType.Blue;
        case 3:
          return RegionType.Green;
        case 4:
          return RegionType.Yellow;
        case 5:
          return RegionType.Purple;
        case 6:
          return RegionType.Brown;
        case 0:
          return RegionType.Cyan;
        case 8:
          return RegionType.Lime;
        case 9:
          return RegionType.Rose;
        case 10:
          return RegionType.Mint;
        default:
          throw new Exception($"Cant GetFraction for: {hex.fraction}");
      }
    }

    private List<RegionPureData> CreateRegions(CellPureData?[,] cells)
    {
      var newRegions = new List<RegionPureData>();
      var passed = new HashSet<CellPureData>();
      var front = new Queue<CellPureData>();

      foreach (var rootCellNull in cells)
      {
        if (!rootCellNull.HasValue)
          continue;

        var rootCell = rootCellNull.Value;

        if (passed.Contains(rootCell))
          continue;

        var newRegion = new RegionPureData
        {
          Cells = new List<CellPureData>(),
          Type = rootCell.RegionType,
          Money = 10
        };
        newRegions.Add(newRegion);

        front.Enqueue(rootCell);
        passed.Add(rootCell);
        newRegion.Cells.Add(rootCell);

        while (front.Count > 0)
        {
          var cell = front.Dequeue();

          foreach (var neighbour in GetNeighbourCells(cells, cell))
          {
            if (neighbour.RegionType == rootCell.RegionType && !passed.Contains(neighbour))
            {
              newRegion.Cells.Add(neighbour);
              front.Enqueue(neighbour);
              passed.Add(neighbour);
            }
          }
        }
      }

      return newRegions;
    }

    private IEnumerable<CellPureData> GetNeighbourCells(CellPureData?[,] cells, CellPureData cell)
    {
      var aroundPosition = cell.Position;
      var maxX = cells.GetLength(0);
      var maxY = cells.GetLength(1);

      foreach (var direction in HexUtilities.Directions)
      {
        var index = (aroundPosition + direction).ToArray2DIndex();
        if (index.x >= 0 && index.x < maxX && index.y >= 0 && index.y < maxY)
        {
          var item = cells[index.x, index.y];
          if (item.HasValue)
            yield return item.Value;
        }
      }
    }

    private List<PlayerPureData> CreatePlayers(List<RegionPureData> regions)
    {
      var players = new List<PlayerPureData>();

      while (regions.Count > 0)
      {
        var rootRegion = regions[0];
        regions.RemoveAt(0);

        var player = new PlayerPureData
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

    private void CreateCapitals(List<RegionPureData> regions)
    {
      foreach (var region in regions)
      {
        if (region.Type != RegionType.Neutral)
        {
          var cell = (GetRandomFreeCell(region.Cells) ?? GetRandomCellExceptTowers(region.Cells)) ??
                     region.Cells[Random.Range(0, region.Cells.Count)];
          cell.Unit = new UnitPureData
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

    private CellPureData? GetRandomFreeCell(List<CellPureData> cells)
    {
      using (ListPool<CellPureData>.Get(out var resultCells))
      {
        foreach (var cell in cells)
          if (!cell.Unit.HasValue)
            resultCells.Add(cell);
        return resultCells.Count == 0 ? null : resultCells[Random.Range(0, resultCells.Count)];
      }
    }

    private CellPureData? GetRandomCellExceptTowers(List<CellPureData> cells)
    {
      using (ListPool<CellPureData>.Get(out var resultCells))
      {
        foreach (var cell in cells)
          if (!cell.Unit.HasValue || cell.Unit.Value.Type != UnitType.Tower)
            resultCells.Add(cell);
        return resultCells.Count == 0 ? null : resultCells[Random.Range(0, resultCells.Count)];
      }
    }
  }
}