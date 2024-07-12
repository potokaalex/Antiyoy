using System;
using UnityEngine;

namespace Code.Infrastructure
{
    [Serializable]
    public class ConfigProvider
    {
        [SerializeField] private ScenesConfig _scenesConfig;

        public ScenesConfig GetScenes() => _scenesConfig;
    }
}