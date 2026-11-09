using System.Collections.Generic;
using System.Linq;
using Client.Project.Infrastructure;
using Client.Project.Progress;
using UnityEngine;

namespace Client.Project.Map
{
  public class MapsService : IInitializable
  {
    private readonly List<MapController> _maps = new();
    private readonly List<MapController> _editorMaps = new();
    private readonly MapGenerator _mapGenerator = new();
    private ProgressController _progressController;
    private MapsFactory _mapsFactory;
    private MapDecodeController _mapDecodeController;

    public MapController StartMap { get; private set; }

    private MapController AutosaveMap { get; set; }

    public IReadOnlyList<MapController> Maps => _maps;

    public IReadOnlyList<MapController> EditorMaps => _editorMaps;

    public void Initialize()
    {
      _progressController = Locator.Get<ProgressController>();
      _mapsFactory = Locator.Get<MapsFactory>();
      _mapDecodeController = Locator.Get<MapDecodeController>();
      Load();
    }

    public void GenerateStartMap(LevelSize levelSize, int colorsCount, string name) =>
      StartMap = _mapGenerator.Generate(levelSize, colorsCount, name);

    public void GenerateEmptyStartMap(LevelSize levelSize, string name) => StartMap = _mapGenerator.GenerateEmpty(levelSize, name);

    public bool GenerateStartMapFromCopyBuffer()
    {
      var code = GUIUtility.systemCopyBuffer;
      if (_mapDecodeController.IsValidLevelCode(code))
      {
        StartMap = new MapController(_mapDecodeController.DecodeToMapData(code));
        return true;
      }

      return false;
    }

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
      var mapData = _mapsFactory.CreateRuntimeMap(StartMap.GetData().InitialPlayers, StartMap.Name, isEditor: isEditor, isAutosave: isAutosave);
      var map = new MapController(mapData);
      if (isEditor)
      {
        _progressController.Data.EditorMaps[slotIndex] = mapData;
        _editorMaps[slotIndex] = map;
      }
      else
      {
        _progressController.Data.Maps[slotIndex] = mapData;
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