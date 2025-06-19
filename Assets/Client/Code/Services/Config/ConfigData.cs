using System.Collections.Generic;
using ClientCode.Gameplay.Cell;
using ClientCode.Services.SceneLoader;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client.Code.Services.Config
{
    [CreateAssetMenu(menuName = "Client/Configs/Main", fileName = "ConfigData", order = 0)]
    public class ConfigData : SerializedScriptableObject
    {
        public Dictionary<SceneName, string> Scenes;
        public CellDebugBehaviour CellDebugPrefab;
    }
}