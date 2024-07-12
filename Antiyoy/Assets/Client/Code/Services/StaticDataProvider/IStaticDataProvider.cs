using System.Collections.Generic;

namespace ClientCode.Services.StaticDataProvider
{
    public interface IStaticDataProvider
    {
        void Initialize(List<IStaticData> configs);
        T Get<T>() where T : IStaticData;
    }
}