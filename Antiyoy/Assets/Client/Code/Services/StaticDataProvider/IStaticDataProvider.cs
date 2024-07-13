using ClientCode.Data.Static;

namespace ClientCode.Services.StaticDataProvider
{
    public interface IStaticDataProvider
    {
        void Initialize(Configs configs, Prefabs prefabs);
        ProjectLoadData ProjectLoadData { get; }
        Configs Configs { get; }
        Prefabs Prefabs { get; }
    }
}