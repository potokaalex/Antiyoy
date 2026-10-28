using System;
using System.Collections.Generic;
using Antiyoy.Gameplay;
using Client.Project.Hex;
using Client.Project.Map;
using Client.Project.Region;
using Client.Project.Unit.Code;
using UnityEngine;

namespace Client._T
{
  public class MapGeneratorT
  {
    public MapPureData Generate(LevelSize levelSize, int playersCount)
    {
      var map = new MapPureData();
      map.Size = new Vector2Int(20, 20);

      GameRules.fractionsQuantity = playersCount;
      var gameController = new GameController(map.Size.x, map.Size.y);
      var mapGeneratorGeneric = new MapGeneratorGeneric(gameController);
      gameController.fieldManager.createFieldMatrix();
      mapGeneratorGeneric.generateMap(new System.Random(Environment.TickCount), gameController.fieldManager.field);

      var cells = CreateCells(gameController.fieldManager.field);
      var regions = CreateRegions(cells);
      map.Players = CreatePlayers(regions);

      return map;
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
        if(!rootCellNull.HasValue)
          continue;

        var rootCell = rootCellNull.Value;

        if (passed.Contains(rootCell))
          continue;

        var newRegion = new RegionPureData
        {
          Cells = new List<CellPureData>(),
          Type = rootCell.RegionType,
          Money = 100
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
      foreach (var direction in HexUtilities.Directions)
      {
        var index = (aroundPosition + direction).ToArray2DIndex();
        var item = cells[index.x, index.y];
        if(item.HasValue)
          yield return item.Value;
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
  }
}