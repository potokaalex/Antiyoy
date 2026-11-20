using System;
using System.IO;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Client.Project.Progress
{
  public class ProgressController : IInitializable
  {
    private const string _filesExtension = "data";
    private const string _saveFolderName = "Saves";
    private JsonSerializerSettings _jsonSettings;
    private string _directoryPath;
    private string _filePath;

    public ProgressData Data { get; private set; }

    public void Initialize()
    {
      _jsonSettings = new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Include,
      };
      CreatePaths();
      Load();
    }

    public void Clear()
    {
      try
      {
        Directory.Delete(_directoryPath, true);
      }
      catch (Exception e)
      {
        Debug.Log(e.Message);
      }
    }

    public void Save() => SaveProgress(Data);

    public void SaveMap(MapData mapData) => SaveData(GetMapPath(mapData.Id), mapData);

    public bool LoadMap(int mapId, out MapData mapData)
    {
      mapData = LoadData<MapData>(GetMapPath(mapId), null);
      return mapData != null;
    }

    public void ClearMap(int mapId)
    {
      try
      {
        File.Delete(GetMapPath(mapId));
      }
      catch (Exception e)
      {
        Debug.Log(e.Message);
      }
    }

    private void Load() => Data ??= LoadProgress();

    private ProgressData LoadProgress() => LoadData(_filePath, new ProgressData());

    private void SaveProgress(ProgressData progress) => SaveData(_filePath, progress);

    private void CreatePaths()
    {
      _directoryPath = Path.Combine(GetBaseDirectoryPath(), _saveFolderName);
      _filePath = Path.Combine(_directoryPath, Path.ChangeExtension(nameof(ProgressData), _filesExtension));

      if (!Directory.Exists(_directoryPath))
        Directory.CreateDirectory(_directoryPath);
    }

    private string GetBaseDirectoryPath()
    {
#if UNITY_EDITOR
      return Directory.GetParent(Application.dataPath)!.FullName;
#else
      return Application.persistentDataPath;
#endif
    }

    private T LoadData<T>(string path, T def)
    {
      if (!File.Exists(path))
        return def;

      try
      {
        using var reader = new StreamReader(path, false);
        var result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), _jsonSettings);
        if (result != null)
          return result;
      }
      catch (Exception e)
      {
        Debug.Log(e.Message);
      }

      return def;
    }

    private void SaveData(string path, object data)
    {
      try
      {
        using var writer = new StreamWriter(path, false);
        writer.Write(JsonConvert.SerializeObject(data, _jsonSettings));
      }
      catch (Exception e)
      {
        Debug.Log(e.Message);
      }
    }

    private string GetMapPath(int mapId) => Path.Combine(_directoryPath, Path.ChangeExtension($"Map_{mapId}", _filesExtension));
  }
}