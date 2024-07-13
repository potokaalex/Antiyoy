using ClientCode.Data.Static;

namespace ClientCode.Services.StaticDataProvider
{
    public interface IStaticDataProvider
    {
        Configs Configs { get; }
        Prefabs Prefabs { get; }
        void Initialize(Configs configs, Prefabs prefabs);
    }
}