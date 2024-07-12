using ClientCode.Services.StaticDataProvider;
using UnityEngine;

namespace ClientCode.Data.Configs
{
    [CreateAssetMenu(menuName = "Configs/Scenes", fileName = "ScenesConfig", order = 0)]
    public class SceneConfig : ScriptableObject, IStaticData
    {
        public string BootstrapSceneName;
        public string MainMenuSceneName;
        public string MapEditorSceneName;
        public string GameplaySceneName;
    }
}