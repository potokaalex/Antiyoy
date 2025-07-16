using System;
using System.Collections.Generic;
using System.IO;
using Client.Code.Core.Progress.Actors;
using Client.Code.Core.UnityEvents.Events;
using UnityEngine;
using Zenject;

namespace Client.Code.Core.Progress
{
    public class ProgressController : IInitializable, IOnApplicationFocusReceiver, IProgressProvider
    {
        private const string FilesExtension = "data";
        private const string SaveFolderName = "Saves";
        private readonly List<IProgressWriter> _writers = new();
        private ProgressData _progressData;
        private string _directoryPath;
        private string _filePath;

        ProgressData IProgressProvider.Data => _progressData;

        public void Initialize()
        {
            CreatePaths();
            Load();
        }

        public void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                Save();
        }

        public void RegisterActor(IProgressActor actor)
        {
            if (actor is IProgressWriter writer)
                _writers.Add(writer);
        }

        public void UnRegisterActor(IProgressActor actor)
        {
            if (actor is IProgressWriter writer)
                _writers.Remove(writer);
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

        private void Load() => _progressData ??= LoadProgress();

        private void Save()
        {
            ActorsWriteProgress();
            SaveProgress(_progressData);
        }

        private void ActorsWriteProgress()
        {
            foreach (var writer in _writers)
                writer.OnWrite(_progressData);
        }

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
            _directoryPath = Path.Combine(GetBaseDirectoryPath(), SaveFolderName);
            _filePath = Path.Combine(_directoryPath, Path.ChangeExtension(nameof(ProgressData), FilesExtension));
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