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
    private MapController _autosaveMap;
    private MapController _startMap;

    public IReadOnlyMap StartMap => _startMap;

    public void Initialize()
    {
      _mapGenerator = new();
      _progressController = Locator.Get<ProgressController>();
      _mapsFactory = Locator.Get<MapsFactory>();
      _mapDecodeController = Locator.Get<MapDecodeController>();
      _mapEncodeController = Locator.Get<MapEncodeController>();
      Load();
    }

    public IEnumerable<IReadOnlyMap> GetMaps()
    {
      foreach (var map in _maps)
          yield return map;
    }

    //создаёт начальную карту без слота.
    public void GenerateStartMap(LevelSize levelSize, int colorsCount, string name) => _startMap = _mapGenerator.Generate(levelSize, colorsCount, name);

    //изменяет данные начальной карты.
    public void GenerateMapIntoStart(LevelSize levelSize, int colorsCount)
    {
      var data = _mapGenerator.GenerateData(levelSize, colorsCount, null);
      _startMap.SetSize(data.Size);
      _startMap.SetPlayers(data.Players);
    }

    //создаёт начальную карту без слота.
    public void GenerateEmptyStartMap(LevelSize levelSize, string name) => _startMap = _mapGenerator.GenerateEmpty(levelSize, name);

    public bool CanGenerateFromCopyBuffer() => _mapDecodeController.IsValidLevelCode(GUIUtility.systemCopyBuffer);

    //создаёт начальную карту без слота.
    public void GenerateStartMapFromCopyBuffer() => _startMap = new MapController(_mapDecodeController.DecodeToMapData(GUIUtility.systemCopyBuffer));

    //изменяет данные начальной карты.
    public void GenerateFromCopyBufferIntoStartMap()
    {
      var data = _mapDecodeController.DecodeToMapData(GUIUtility.systemCopyBuffer);
      _startMap.SetSize(data.Size);
      _startMap.SetPlayers(data.Players);
    }

    //устанавливает начальную карту, т.е. карта может быть без слота. НЕТ, мы можем только из слота это делать.
    public void SetStartMapFromSlot(int id) => _startMap = _maps[id];

    //переименовывает карту.
    public void RenameMap(int id, string name)
    {
      _maps[id].Name = name;
      _progressController.Save();
    }

    //удалаяет карту.
    public void DeleteMap(int id) => DeleteMap(_maps[id]);

    //сохраняет карту в новый слот.

    public IReadOnlyMap SaveRuntimeMapInNewSlot()
    {
      InsertNullMap();
      return SaveRuntimeMapInSlotInternal(0);
    }

    //сохраняет карту в слот.

    public IReadOnlyMap SaveRuntimeMapInSlot(int id) =>
      SaveRuntimeMapInSlotInternal(id);//поидее не должно пересоздавать карту.

    //сохраняет карту в слот автосейва.

    public void SaveRuntimeMapInAutosave()
    {
      if (_autosaveMap != null)
        DeleteMap(_autosaveMap);

      InsertNullMap();
      _autosaveMap = SaveRuntimeMapInSlotInternal(0, isAutosave: true);
    }

    //сохраняет карту либо в слот, либо в новый слот.

    public void SaveRuntimeEditorMap()
    {
      var index = _maps.IndexOf(_startMap);
      if (index == -1)
      {
        InsertNullMap();
        index = 0;
      }

      _startMap = SaveRuntimeMapInSlotInternal(index, isEditor: true);
    }

    public void RestartStartMap() => _startMap.Restart();

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
      _autosaveMap = _maps.FirstOrDefault(x => x.IsAutosave);
    }

    private MapController SaveRuntimeMapInSlotInternal(int slotIndex, bool isEditor = false, bool isAutosave = false)
    {
      var mapData = _mapsFactory.CreateRuntimeMap(_startMap.GetData().InitialPlayers, StartMap.Name, isEditor: isEditor, isAutosave: isAutosave);
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

    private void DeleteMap(MapController map)
    {
      _maps.Remove(map);
      _progressController.ClearMap(map.Id);
      _progressController.Data.MapsIds.Remove(map.Id);
      _progressController.Save();
    }
  }
}