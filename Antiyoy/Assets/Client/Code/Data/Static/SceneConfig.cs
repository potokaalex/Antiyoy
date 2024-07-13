using UnityEngine;

namespace ClientCode.Data.Static
{
    [CreateAssetMenu(menuName = "Configs/Scenes", fileName = "ScenesConfig", order = 0)]
    public class SceneConfig : ScriptableObject
    {
        public string BootstrapSceneName;
        public string MainMenuSceneName;
        public string MapEditorSceneName;
        public string GameplaySceneName;
    }
}