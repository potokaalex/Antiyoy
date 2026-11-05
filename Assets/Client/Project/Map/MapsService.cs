using System;
using System.Collections.Generic;
using System.Linq;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Client.Project.Progress;
using Client.Project.Region;
using UnityEngine;

namespace Client.Project.Map
{
  public class MapsService : IInitializable
  {
    private readonly List<MapController> _maps = new();
    private readonly List<MapController> _editorMaps = new();
    private readonly MapGenerator _mapGenerator = new();
    private ProgressController _progressController;
    private GridController _gridController;
    private GovernmentsService _governmentsService;

    public MapController StartMap { get; private set; }

    private MapController AutosaveMap { get; set; }

    public IReadOnlyList<MapController> Maps => _maps;

    public IReadOnlyList<MapController> EditorMaps => _editorMaps;

    public void Initialize()
    {
      _progressController = Locator.Get<ProgressController>();
      _gridController = Locator.Get<GridController>();
      _governmentsService = Locator.Get<GovernmentsService>();
      Load();
    }

    public void GenerateStartMap(LevelSize levelSize, int colorsCount, string name) =>
      StartMap = _mapGenerator.Generate(levelSize, colorsCount, name);

    public void GenerateEmptyStartMap(LevelSize levelSize, string name) => StartMap = _mapGenerator.GenerateEmpty(levelSize, name);

    public void SetStartMap(MapController map) => StartMap = map;

    public void RenameMap(MapController map, string name)
    {
      map.Name = name;
      _progressController.Save();
    }

    public void DeleteMap(MapController map)
    {
      DeleteMapWithoutSave(map);
      _progressController.Save();
    }

    public void DeleteEditorMap(MapController map)
    {
      _editorMaps.Remove(map);
      _progressController.Data.EditorMaps.Remove(map.GetData());
      _progressController.Save();
    }

    public MapController SaveCurrentMapInNewSlot()
    {
      InsertNullMap();
      return SaveCurrentMapInSlot(0);
    }

    public MapController SaveCurrentMapInSlot(MapController mapController) =>
      SaveCurrentMapInSlot(_maps.IndexOf(mapController));

    public void SaveCurrentMapInAutosave()
    {
      if (AutosaveMap == null)
        InsertNullMap();

      AutosaveMap = SaveCurrentMapInSlot(0, isAutosave: true);
    }

    public void SaveCurrentEditorMap()
    {
      var index = _editorMaps.IndexOf(StartMap);
      if (index == -1)
      {
        InsertNullMap(isEditor: true);
        index = 0;
      }

      StartMap = SaveCurrentMapInSlot(index, isEditor: true);
    }

    public void RestartStartMap() => StartMap = StartMap.RestartCopy();

    public LevelSize GetLevelSize(Vector2Int mapSize) => _mapGenerator.GetLevelSize(mapSize);

    private MapController CreateCurrentMap(bool isAutosave = false, bool isEditor = false)
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
        InitialPlayers = isEditor ? players : StartMap.GetData().InitialPlayers,
        Players = players,
        Size = _gridController.Size,
        Name = StartMap.Name,
        CreationTime = DateTime.Now.ToBinary(),
        IsAutosave = isAutosave
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
      foreach (var mapData in _progressController.Data.Maps)
        _maps.Add(new MapController(mapData));

      AutosaveMap = _maps.FirstOrDefault(x => x.IsAutosave);

      foreach (var mapData in _progressController.Data.EditorMaps)
        _editorMaps.Add(new MapController(mapData));
    }

    private void DeleteMapWithoutSave(MapController map)
    {
      _maps.Remove(map);
      _progressController.Data.Maps.Remove(map.GetData());
    }

    private MapController SaveCurrentMapInSlot(int slotIndex, bool isEditor = false, bool isAutosave = false)
    {
      var map = CreateCurrentMap(isEditor: isEditor, isAutosave: isAutosave);
      if (isEditor)
      {
        _progressController.Data.EditorMaps[slotIndex] = map.GetData();
        _editorMaps[slotIndex] = map;
      }
      else
      {
        _progressController.Data.Maps[slotIndex] = map.GetData();
        _maps[slotIndex] = map;
      }

      _progressController.Save();
      return map;
    }

    private void InsertNullMap(bool isEditor = false)
    {
      if (isEditor)
      {
        _editorMaps.Insert(0, null);
        _progressController.Data.EditorMaps.Insert(0, null);
      }
      else
      {
        _maps.Insert(0, null);
        _progressController.Data.Maps.Insert(0, null);
      }
    }
  }
}