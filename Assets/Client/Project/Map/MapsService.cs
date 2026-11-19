using System.Collections.Generic;
using System.Linq;
using Client.Project.Infrastructure;
using Client.Project.Player;
using Client.Project.Progress;
using UnityEngine;

namespace Client.Project.Map
{
  public class MapsService : IInitializable
  {
    private readonly List<MapController> _maps = new();
    private MapGenerator _mapGenerator;
    private MapDecodeController _mapDecodeController;
    private MapEncodeController _mapEncodeController;
    private ProgressController _progressController;
    private MapsFactory _mapsFactory;
    private MapController _startMap;
    private PlayersService _playersService;

    public IReadOnlyMap StartMap => _startMap;

    public void Initialize()
    {
      _progressController = Locator.Get<ProgressController>();
      _mapsFactory = Locator.Get<MapsFactory>();
      _playersService = Locator.Get<PlayersService>();
      _mapGenerator = new();
      _mapDecodeController = new();
      _mapEncodeController = new();
      _startMap = new();
      Load();
    }

    public IReadOnlyList<IReadOnlyMap> GetMaps() => _maps;

    public void GenerateStartMap(LevelSize levelSize, int regionsCount, string name, int difficultyMode, int playersMode) =>
      _startMap.Data = _mapGenerator.GenerateDataAndFirstPlayer(levelSize, regionsCount, name, difficultyMode, playersMode);

    public void GenerateMapIntoStart(LevelSize levelSize, int colorsCount) => _startMap.Data =
      _mapGenerator.GenerateData(levelSize, colorsCount, _startMap.Name, _playersService.DifficultyMode, _playersService.PlayersMode, 
        _playersService.FirstPlayer, _startMap.Id);

    public void GenerateEmptyStartMap(LevelSize levelSize, string name) => _startMap.Data = _mapGenerator.GenerateEmptyData(levelSize, name);

    public bool CanGenerateFromCopyBuffer() => _mapDecodeController.IsValidLevelCode(GUIUtility.systemCopyBuffer);

    public void GenerateStartMapFromCopyBuffer() => _startMap.Data = _mapDecodeController.DecodeToMapData(GUIUtility.systemCopyBuffer);

    public void GenerateFromCopyBufferIntoStartMap() =>
      _startMap.Data = _mapDecodeController.DecodeToMapData(GUIUtility.systemCopyBuffer, _startMap.Id, _startMap.Name);

    public void SetStartMapFromSlot(int mapId) => SetStartMap(GetMap(mapId));

    public void RenameMap(int mapId, string name)
    {
      GetMap(mapId).Name = name;
      _progressController.Save();
    }

    public void DeleteMap(int mapId) => DeleteMap(GetMap(mapId));

    public IReadOnlyMap SaveRuntimeMapInNewSlot() => SaveRuntimeMapInSlotInternal(-1);

    public IReadOnlyMap SaveRuntimeMapInSlot(int mapId) => SaveRuntimeMapInSlotInternal(mapId);

    public void SaveRuntimeMapInAutosave()
    {
      var autosave = _maps.FirstOrDefault(x => x.IsAutosave);
      if (autosave != null)
        DeleteMap(autosave, false);

      SaveRuntimeMapInSlotInternal(-1, isAutosave: true);
    }

    public void SaveRuntimeEditorMap() => SetStartMap(SaveRuntimeMapInSlotInternal(_startMap.Id, isEditor: true));

    public void RestartStartMap() => _startMap.Restart();

    public void ExportFromRuntime() => GUIUtility.systemCopyBuffer = _mapEncodeController.EncodeFromRuntime();

    private void Load()
    {
      foreach (int id in _progressController.Data.MapsIds)
        if (_maps.All(x => x.Id != id) && _progressController.LoadMap(id, out var mapData))
          _maps.Add(new MapController(mapData));
      UpdateIds();
    }

    private MapController SaveRuntimeMapInSlotInternal(int mapId, bool isEditor = false, bool isAutosave = false)
    {
      MapController map;
      if (mapId == -1)
      {
        mapId = _mapsFactory.GenerateId(_progressController.Data.MapsIds);
        map = new MapController();
        _maps.Insert(0, map);
      }
      else
        map = GetMap(mapId);

      var mapData = _mapsFactory.CreateRuntimeMap(_startMap.Data.InitialPlayers, StartMap.Name, isEditor: isEditor, isAutosave: isAutosave,
        id: mapId);
      map.Data = mapData;
      _progressController.SaveMap(mapData);
      UpdateIds();
      return map;
    }

    private void DeleteMap(MapController map, bool saveIds = true)
    {
      if (_maps.Remove(map))
      {
        _progressController.ClearMap(map.Id);
        UpdateIds(saveIds);
      }
    }

    private void UpdateIds(bool saveIds = true)
    {
      var ids = _progressController.Data.MapsIds;
      ids.Clear();
      foreach (var map in _maps)
        ids.Add(map.Id);
      if (saveIds)
        _progressController.Save();
    }

    private void SetStartMap(MapController map) => _startMap.CopyFrom(map);

    private MapController GetMap(int mapId)
    {
      for (var i = 0; i < _maps.Count; i++)
      {
        var map = _maps[i];
        if (map.Id == mapId)
          return map;
      }

      return null;
    }
  }
}