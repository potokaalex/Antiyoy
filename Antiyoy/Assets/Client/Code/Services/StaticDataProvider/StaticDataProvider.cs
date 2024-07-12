using System;
using System.Collections.Generic;

namespace ClientCode.Services.StaticDataProvider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        private Dictionary<Type, IStaticData> _date;

        public void Initialize(List<IStaticData> data)
        {
            foreach (var d in data) 
                _date.Add(d.GetType(), d);
        }

        public T Get<T>() where T : IStaticData => (T)_date[typeof(T)];
    }
}