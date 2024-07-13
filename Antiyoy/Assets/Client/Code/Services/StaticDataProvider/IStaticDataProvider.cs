using ClientCode.Data.Static;

namespace ClientCode.Services.StaticDataProvider
{
    public interface IStaticDataProvider
    {
        void Initialize(Configs configs, Prefabs prefabs);
        Configs Configs { get; }
        Prefabs Prefabs { get; }
    }
}