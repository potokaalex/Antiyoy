using System.Collections.Generic;
using Client.Code.Gameplay.Cell;
using Client.Code.Services.Scene;
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