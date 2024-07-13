using System;
using ClientCode.Data.Static;

namespace ClientCode.Data.Progress.Load
{
    [Serializable]
    public class ProjectLoadData : ILoadData
    {
        public Configs Configs;
        public Prefabs Prefabs;
    }
}