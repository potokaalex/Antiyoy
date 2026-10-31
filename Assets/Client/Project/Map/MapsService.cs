using System;
using System.Collections.Generic;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Client.Project.Progress;
using Client.Project.Region;

namespace Client.Project.Map
{
  public class MapsService : IInitializable
  {
    private readonly List<MapController> _maps = new();
    private readonly MapGenerator _mapGenerator = new();
    private ProgressController _progressController;
    private GridController _gridController;
    private GovernmentsService _governmentsService;

    public MapController StartMap { get; private set; }

    public MapController AutosaveMap { get; private set; }

    public IReadOnlyList<MapController> Maps => _maps;

    public void Initialize()
    {
      _progressController = Locator.Get<ProgressController>();
      _gridController = Locator.Get<GridController>();
      _governmentsService = Locator.Get<GovernmentsService>();
      Load();
    }

    public void GenerateStartMap(LevelSize levelSize, int colorsCount) => StartMap = _mapGenerator.Generate(levelSize, colorsCount);

    public void SetStartMap(MapController map) => StartMap = map;

    public void RenameMap(MapController map, string name)
    {
      map.Name = name;
      _progressController.Save();
    }

    public void DeleteMap(MapController map)
    {
      _maps.Remove(map);
      _progressController.Data.Maps.Remove(map.GetData());
      _progressController.Save();
    }

    public MapController SaveCurrentMapInNewSlot(string mapName)
    {
      var map = CreateCurrentMap(mapName);
      _progressController.Data.Maps.Insert(0, map.GetData());
      _progressController.Save();
      _maps.Insert(0, map);
      return map;
    }

    public MapController SaveCurrentMapInSlot(MapController mapController, string mapName)
    {
      var map = CreateCurrentMap(mapName);
      var index = _maps.IndexOf(mapController);
      _progressController.Data.Maps[index] = map.GetData();
      _progressController.Save();
      _maps[index] = map;
      return map;
    }

    private MapController CreateCurrentMap(string mapName)
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

      return new MapController(new MapData
      {
        Players = players,
        Size = _gridController.Size,
        Name = mapName,
        CreationTime = DateTime.Now.ToBinary()
      });
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

    private void Load()
    {
      if (_progressController.Data.AutosaveMap != null)
        AutosaveMap = new MapController(_progressController.Data.AutosaveMap);

      foreach (var mapData in _progressController.Data.Maps)
        _maps.Add(new MapController(mapData));
    }
  }
}