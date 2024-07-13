using ClientCode.Data.Static;

namespace ClientCode.Services.StaticDataProvider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        public StaticDataProvider(ProjectLoadData projectLoadData) => ProjectLoadData = projectLoadData;

        public void Initialize(Configs configs, Prefabs prefabs)
        {
            Prefabs = prefabs;
            Configs = configs;
        }

        public ProjectLoadData ProjectLoadData { get; }

        public Configs Configs { get; private set; }
        
        public Prefabs Prefabs { get; private set; }
    }
}