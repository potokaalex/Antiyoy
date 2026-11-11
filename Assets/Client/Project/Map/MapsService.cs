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
    private readonly MapGenerator _mapGenerator = new();
    private ProgressController _progressController;
    private MapsFactory _mapsFactory;
    private MapDecodeController _mapDecodeController;
    private MapEncodeController _mapEncodeController;

    public MapController StartMap { get; private set; }

    private MapController AutosaveMap { get; set; }

    public void Initialize()
    {
      _progressController = Locator.Get<ProgressController>();
      _mapsFactory = Locator.Get<MapsFactory>();
      _mapDecodeController = Locator.Get<MapDecodeController>();
      _mapEncodeController = Locator.Get<MapEncodeController>();
      Load();
    }

    public IEnumerable<MapController> GetMaps()
    {
      foreach (var map in _maps)
        if (!map.IsEditor)
          yield return map;
    }
    
    public IEnumerable<MapController> GetEditorMaps()
    {
      foreach (var map in _maps)
        if (map.IsEditor)
          yield return map;
    }

    public void GenerateStartMap(LevelSize levelSize, int colorsCount, string name) =>
      StartMap = _mapGenerator.Generate(levelSize, colorsCount, name);

    public void GenerateEmptyStartMap(LevelSize levelSize, string name) => StartMap = _mapGenerator.GenerateEmpty(levelSize, name);

    public void GenerateFromCopyBufferIntoEditorStartMap() => GenerateStartMapFromCopyBuffer(StartMap.Name, _maps.IndexOf(StartMap));

    public bool GenerateStartMapFromCopyBuffer() => GenerateStartMapFromCopyBuffer(null);

    private bool GenerateStartMapFromCopyBuffer(string overrideName, int editorIndex = -1)
    {
      var code = GUIUtility.systemCopyBuffer;
      if (_mapDecodeController.IsValidLevelCode(code))
      {
        StartMap = new MapController(_mapDecodeController.DecodeToMapData(code));
        if (overrideName != null)
          StartMap.Name = overrideName;
        if (editorIndex != -1)
        {
          _maps[editorIndex] = StartMap;
          StartMap.IsEditor = true;
        }
        return true;
      }

      return false;
    }

    public bool CanGenerateStartMapFromCopyBuffer() => _mapDecodeController.IsValidLevelCode(GUIUtility.systemCopyBuffer);

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

    public MapController SaveCurrentMapInNewSlot()
    {
      InsertNullMap();
      return SaveCurrentMapInSlot(0);
    }

    public MapController SaveCurrentMapInSlot(MapController mapController) =>
      SaveCurrentMapInSlot(_maps.IndexOf(mapController));

    public void SaveCurrentMapInAutosave()
    {
      if (AutosaveMap != null)
        DeleteMapWithoutSave(AutosaveMap);

      InsertNullMap();
      AutosaveMap = SaveCurrentMapInSlot(0, isAutosave: true);
    }

    public void SaveCurrentEditorMap()
    {
      var index = _maps.IndexOf(StartMap);
      if (index == -1)
      {
        InsertNullMap();
        index = 0;
      }

      StartMap = SaveCurrentMapInSlot(index, isEditor: true);
    }

    public void RestartStartMap() => StartMap = StartMap.RestartCopy();

    public void ExportFromRuntime() => GUIUtility.systemCopyBuffer = _mapEncodeController.EncodeFromRuntime();

    private void Load()
    {
      foreach (var mapData in _progressController.Data.Maps)
        _maps.Add(new MapController(mapData));

      AutosaveMap = _maps.FirstOrDefault(x => x.IsAutosave);
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
      _progressController.Data.Maps[slotIndex] = mapData;
      _maps[slotIndex] = map;
      _progressController.Save();
      return map;
    }

    private void InsertNullMap()
    {
      _maps.Insert(0, null);
      _progressController.Data.Maps.Insert(0, null);
    }
  }
}