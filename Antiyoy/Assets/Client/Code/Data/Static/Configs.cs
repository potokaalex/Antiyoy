using System;
using ClientCode.Data.Static.Config;

namespace ClientCode.Data.Static
{
    [Serializable]
    public class Configs
    {
        public SceneConfig Scene;
        public ProgressConfig Progress;
        public GameplayConfig Gameplay;
        public UIConfig UI;
    }
}