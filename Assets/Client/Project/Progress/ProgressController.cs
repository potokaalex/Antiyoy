using System;
using System.IO;
using Client.Project.Infrastructure;
using UnityEngine;

namespace Client.Project.Progress
{
  public class ProgressController : IInitializable
  {
    private const string _filesExtension = "data";
    private const string _saveFolderName = "Saves";
    private ProgressData _progressData;
    private string _directoryPath;
    private string _filePath;

    public ProgressData Data => _progressData;

    public void Initialize()
    {
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

    public void Save() => SaveProgress(_progressData);

    private void Load() => _progressData ??= LoadProgress();

    private ProgressData LoadProgress()
    {
      var defaultProgress = new ProgressData();

      if (!File.Exists(_filePath))
        return defaultProgress;

      try
      {
        using var reader = new StreamReader(_filePath, false);
        var readData = reader.ReadToEnd();
        var result = JsonUtility.FromJson<ProgressData>(readData);
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
        writer.Write(JsonUtility.ToJson(progress));
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