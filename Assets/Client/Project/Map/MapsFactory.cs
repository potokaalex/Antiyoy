using System;
using System.Collections.Generic;
using System.Linq;
using Client.Project.Government;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Client.Project.Region;
using Client.Project.Utilities;
using UnityEngine;

namespace Client.Project.Map
{
  public class MapsFactory : IInitializable
  {
    private readonly HashSet<CellData> _passedBuffer = new();
    private readonly Queue<CellData> _frontBuffer = new();
    private GovernmentsService _governmentsService;
    private GridController _gridController;

    public void Initialize()
    {
      _governmentsService = Locator.Get<GovernmentsService>();
      _gridController = Locator.Get<GridController>();
    }

    public MapData CreateMapData(List<PlayerData> initialPlayers, Vector2Int size, string name, DateTime creationTime = default)
    {
      return new MapData
      {
        InitialPlayers = initialPlayers,
        Players = CopyPlayers(initialPlayers),
        Size = size,
        Name = name,
        CreationTime = creationTime.ToBinary()
      };
    }

    public MapData CreateRuntimeMap(List<PlayerData> initialPlayers, string name, bool isAutosave = false, bool isEditor = false)
    {
      var players = new List<PlayerData>();

      foreach (var government in _governmentsService.GetAll())
      {
        var regions = new List<RegionData>();

        foreach (var r in government.Regions)
        {
          regions.Add(new RegionData
          {
            Cells = CreateCells(r),
            Type = r.Type,
            Money = r.Money
          });
        }

        players.Add(new PlayerData
        {
          Regions = regions
        });
      }

      return new MapData
      {
        InitialPlayers = isEditor ? players : initialPlayers,
        Players = players,
        Size = _gridController.Size,
        Name = name,
        CreationTime = DateTime.Now.ToBinary(),
        IsAutosave = isAutosave,
        IsEditor = isEditor
      };
    }

    public List<RegionData> CreateRegions(CellData?[,] cells, Vector2Int size, int initialMoney)
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
          Money = initialMoney
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

    public List<PlayerData> CreatePlayers(List<RegionData> regions)
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

    public void SetUnit(UnitData? unit, RegionData region, HexCoordinates cellPosition)
    {
      var cells = region.Cells;
      for (var i = 0; i < cells.Count; i++)
      {
        ref var cell = ref cells.GetRef(i);
        if (cell.Position == cellPosition)
        {
          cell.Unit = unit;
          return;
        }
      }
    }

    public void SetUnit(UnitData? unit, CellData?[,] cells, int x, int y)
    {
      ref var cellNullable = ref cells[x, y];

      if (cellNullable.HasValue)
      {
        var cell = cellNullable.Value;
        cell.Unit = unit;
        cells[x, y] = cell;
      }
    }

    public ref RegionData GetRegionByCell(HexCoordinates hex, List<RegionData> regions)
    {
      for (var i = 0; i < regions.Count; i++)
      {
        var cells = regions[i].Cells;
        for (var j = 0; j < cells.Count; j++)
          if (cells[j].Position == hex)
            return ref regions.GetRef(i);
      }

      throw new Exception("Cant get region by cell");
    }

    private List<PlayerData> CopyPlayers(List<PlayerData> players) => players.Select(x => x.Copy()).ToList();

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

    private List<CellData> CreateCells(RegionController region)
    {
      var result = new List<CellData>();
      foreach (var cell in region.Cells)
      {
        UnitData? unit;

        if (cell.HasUnit)
        {
          unit = new UnitData
          {
            Type = cell.Unit.Type,
            HasTurns = cell.Unit.HasTurns
          };
        }
        else
          unit = null;

        result.Add(new CellData
        {
          Position = cell.Position,
          RegionType = cell.Region.Type,
          Unit = unit
        });
      }

      return result;
    }
  }
}