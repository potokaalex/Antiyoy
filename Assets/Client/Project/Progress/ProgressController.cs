using System;
using System.IO;
using Client.Project.Infrastructure;
using UnityEngine;
using Newtonsoft.Json;

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

    private void Load() => Data ??= LoadProgress();

    private ProgressData LoadProgress()
    {
      var defaultProgress = new ProgressData();

      if (!File.Exists(_filePath))
        return defaultProgress;

      try
      {
        using var reader = new StreamReader(_filePath, false);
        var readData = reader.ReadToEnd();
        var result = JsonConvert.DeserializeObject<ProgressData>(readData, _jsonSettings);
        if (result != null)
          return result;
      }
      catch (Exception e)
      {
        Debug.Log(e.Message);
      }

      return defaultProgress;
    }

    private void SaveProgress(ProgressData progress)
    {
      if (!Directory.Exists(_directoryPath))
        Directory.CreateDirectory(_directoryPath);

      try
      {
        using var writer = new StreamWriter(_filePath, false);
        writer.Write(JsonConvert.SerializeObject(progress, _jsonSettings));
      }
      catch (Exception e)
      {
        Debug.Log(e.Message);
      }
    }

    private void CreatePaths()
    {
      _directoryPath = Path.Combine(GetBaseDirectoryPath(), _saveFolderName);
      _filePath = Path.Combine(_directoryPath, Path.ChangeExtension(nameof(ProgressData), _filesExtension));
    }

    private string GetBaseDirectoryPath()
    {
#if UNITY_EDITOR
      return Directory.GetParent(Application.dataPath)!.FullName;
#else
      return Application.persistentDataPath;
#endif
    }
  }
}