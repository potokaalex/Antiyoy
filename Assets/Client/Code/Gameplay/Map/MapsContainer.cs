using System.Collections.Generic;
using UniRx;

namespace ClientCode.UI.Windows.Writing
{
    public class MapsContainer
    {
        private readonly List<MapController> _maps = new();

        public Subject<Unit> OnChangedEvent { get; } = new();

        public void Add(MapController map)
        {
            if (!_maps.Contains(map))
            {
                _maps.Add(map);
                OnChangedEvent.OnNext(default);
            }
        }

        public void Remove(MapController map)
        {
            if (_maps.Remove(map))
                OnChangedEvent.OnNext(default);
        }

        public void Get(List<MapController> outList)
        {
            outList.Clear();
            outList.AddRange(_maps);
        }
    }
}