using UnityEngine;

namespace ClientCode.Data.Static.Config
{
    [CreateAssetMenu(menuName = "Configs/Scene", fileName = "SceneConfig", order = 0)]
    public class SceneConfig : ScriptableObject
    {
        public string BootstrapSceneName;
        public string MainMenuSceneName;
        public string MapEditorSceneName;
        public string GameplaySceneName;
    }
}