using System;
using ClientCode.Data.Static;
using Sirenix.Serialization;

namespace ClientCode.Data.Progress.Project
{
    [Serializable]
    public class ProjectLoadData
    {
        public Configs Configs;
        [NonSerialized, OdinSerialize] public Prefabs Prefabs;
    }
}