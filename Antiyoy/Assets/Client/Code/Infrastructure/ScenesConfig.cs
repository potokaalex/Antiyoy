using UnityEngine;

namespace Code.Infrastructure
{
    [CreateAssetMenu(menuName = "Configs/Scenes", fileName = "ScenesConfig", order = 0)]
    public class ScenesConfig : ScriptableObject
    {
        public string BootstrapSceneName;
        public string MainMenuSceneName;
        public string MapEditorSceneName;
        public string GameplaySceneName;
    }
}