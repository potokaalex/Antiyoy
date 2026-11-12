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
    private MapGenerator _mapGenerator;
    private ProgressController _progressController;
    private MapsFactory _mapsFactory;
    private MapDecodeController _mapDecodeController;
    private MapEncodeController _mapEncodeController;

    public MapController StartMap { get; private set; }

    private MapController AutosaveMap { get; set; }

    public void Initialize()
    {
      _mapGenerator = new();
      _progressController = Locator.Get<ProgressController>();
      _mapsFactory = Locator.Get<MapsFactory>();
      _mapDecodeController = Locator.Get<MapDecodeController>();
      _mapEncodeController = Locator.Get<MapEncodeController>();
      Load();
    }

    public IEnumerable<MapController> GetMaps()
    {
      foreach (var map in _maps)
          yield return map;
    }

    public void GenerateStartMap(LevelSize levelSize, int colorsCount, string name) => StartMap = _mapGenerator.Generate(levelSize, colorsCount, name);

    public void GenerateMapIntoStart(LevelSize levelSize, int colorsCount)
    {
      var data = _mapGenerator.GenerateData(levelSize, colorsCount, null);
      StartMap.SetSize(data.Size);
      StartMap.SetPlayers(data.Players);
    }

    public void GenerateEmptyStartMap(LevelSize levelSize, string name) => StartMap = _mapGenerator.GenerateEmpty(levelSize, name);

    public bool CanGenerateFromCopyBuffer() => _mapDecodeController.IsValidLevelCode(GUIUtility.systemCopyBuffer);

    public void GenerateStartMapFromCopyBuffer() => StartMap = new MapController(_mapDecodeController.DecodeToMapData(GUIUtility.systemCopyBuffer));

    public void GenerateFromCopyBufferIntoStartMap()
    {
      var data = _mapDecodeController.DecodeToMapData(GUIUtility.systemCopyBuffer);
      StartMap.SetSize(data.Size);
      StartMap.SetPlayers(data.Players);
    }

    public void SetStartMap(MapController map) => StartMap = map;

    public void RenameMap(MapController map, string name)
    {
      map.Name = name;
      _progressController.Save();
    }

    public void DeleteMap(MapController map)
    {
      _maps.Remove(map);
      _progressController.ClearMap(map.Id);
      _progressController.Data.MapsIds.Remove(map.Id);
      _progressController.Save();
    }

    public MapController SaveRuntimeMapInNewSlot()
    {
      InsertNullMap();
      return SaveRuntimeMapInSlot(0);
    }

    public MapController SaveRuntimeMapInSlot(MapController mapController) =>
      SaveRuntimeMapInSlot(_maps.IndexOf(mapController));

    public void SaveRuntimeMapInAutosave()
    {
      if (AutosaveMap != null)
        DeleteMap(AutosaveMap);

      InsertNullMap();
      AutosaveMap = SaveRuntimeMapInSlot(0, isAutosave: true);
    }

    public void SaveRuntimeEditorMap()
    {
      var index = _maps.IndexOf(StartMap);
      if (index == -1)
      {
        InsertNullMap();
        index = 0;
      }

      StartMap = SaveRuntimeMapInSlot(index, isEditor: true);
    }

    public void RestartStartMap() => StartMap.Restart();

    public void ExportFromRuntime() => GUIUtility.systemCopyBuffer = _mapEncodeController.EncodeFromRuntime();

    private void Load()
    {
      var mapIds = _progressController.Data.MapsIds;
      for (var i = mapIds.Count - 1; i >= 0; i--)
      {
        var mapId = mapIds[i];
        if (!_progressController.LoadMap(mapId, out var mapData))
        {
          mapIds.RemoveAt(i);
          continue;
        }

        _maps.Insert(0, new MapController(mapData));
      }

      _progressController.Save();
      AutosaveMap = _maps.FirstOrDefault(x => x.IsAutosave);
    }

    private MapController SaveRuntimeMapInSlot(int slotIndex, bool isEditor = false, bool isAutosave = false)
    {
      var mapData = _mapsFactory.CreateRuntimeMap(StartMap.GetData().InitialPlayers, StartMap.Name, isEditor: isEditor, isAutosave: isAutosave);
      var mapsIds = _progressController.Data.MapsIds;
      var oldMap = _maps[slotIndex];
      var id = oldMap?.Id ?? _mapsFactory.GenerateId(mapsIds);
      mapData.Id = id;
      var map = new MapController(mapData);
      _maps[slotIndex] = map;
      _progressController.SaveMap(mapData);
      mapsIds[slotIndex] = mapData.Id;
      _progressController.Save();
      return map;
    }

    private void InsertNullMap()
    {
      _maps.Insert(0, null);
      _progressController.Data.MapsIds.Insert(0, -1);
    }
  }
}