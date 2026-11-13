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
    public readonly UnsafeNullable<CellData>[,] CellsBuffer;
    private readonly HashSet<CellData> _passedBuffer = new(CellDataEqualityComparer.Instance);
    private readonly Queue<CellData> _frontBuffer = new();
    private GovernmentsService _governmentsService;
    private GridController _gridController;

    public MapsFactory()
    {
      var maxFieldSize = GameUtilities.MaxFieldSize;
      CellsBuffer = new UnsafeNullable<CellData>[maxFieldSize.x, maxFieldSize.y];
    }
 
    public void Initialize()
    {
      _governmentsService = Locator.Get<GovernmentsService>();
      _gridController = Locator.Get<GridController>();
    }

    public MapData CreateMapData(List<PlayerData> initialPlayers, Vector2Int size, string name, DateTime creationTime = default, int id = -1)
    {
      return new MapData
      {
        Id = id,
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

    public List<RegionData> CreateRegions(UnsafeNullable<CellData>[,] cells, Vector2Int size, int initialMoney)
    {
      var newRegions = new List<RegionData>();
      _passedBuffer.Clear();
      _frontBuffer.Clear();

      for (var x = 0; x < size.x; x++)
      for (var y = 0; y < size.y; y++)
      {
        ref var rootCellNull = ref cells[x, y];

        if (!rootCellNull.HasValue)
          continue;

        ref var rootCell = ref rootCellNull.Value;

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

          var aroundPosition = cell.Position;

          for (var i = 0; i < HexUtilities.Directions.Length; i++)
          {
            var index = aroundPosition.AddOffsetAndConvertToArray2DIndex(HexUtilities.Directions[i]);
            if (index.x >= 0 && index.x < size.x && index.y >= 0 && index.y < size.y)
            {
              ref var neighbourNull = ref cells[index.x, index.y];
              if (neighbourNull.HasValue)
              {
                ref var neighbour = ref neighbourNull.Value;
                if (neighbour.RegionType == rootCell.RegionType && !_passedBuffer.Contains(neighbour))
                {
                  newRegion.Cells.Add(neighbour);
                  _frontBuffer.Enqueue(neighbour);
                  _passedBuffer.Add(neighbour);
                }
              }
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

    public void SetUnit(UnsafeNullable<UnitData> unit, RegionData region, HexCoordinates cellPosition)
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

    public void SetUnit(UnsafeNullable<UnitData> unit, UnsafeNullable<CellData>[,] cells, int x, int y)
    {
      ref var cellNullable = ref cells[x, y];
      if (cellNullable.HasValue) 
        cellNullable.Value.Unit = unit;
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

    public int GenerateId(List<int> existedIds)
    {
      for (var i = 0; i < int.MaxValue; i++)
      {
        if (!existedIds.Contains(i))
          return i;
      }

      throw new Exception("Cant generate id");
    }

    public void ClearCellsBuffer() => Array.Clear(CellsBuffer, 0, CellsBuffer.Length);

    private List<PlayerData> CopyPlayers(List<PlayerData> players) => players.Select(x => x.Copy()).ToList();

    private List<CellData> CreateCells(RegionController region)
    {
      var result = new List<CellData>();
      foreach (var cell in region.Cells)
      {
        var unit = cell.HasUnit ? new UnsafeNullable<UnitData>(new UnitData(cell.Unit.Type, cell.Unit.HasTurns)) : default;

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