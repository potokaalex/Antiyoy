using System.Collections.Generic;
using UniRx;

namespace ClientCode.UI.Windows.Writing
{
    public class MapsContainer
    {
        private readonly List<MapController> _maps = new();

        public Subject<Unit> OnAddMap { get; } = new();

        public Subject<Unit> OnRemoveMap { get; } = new();

        public MapController CurrentMap { get; set; }

        public void Add(MapController map)
        {
            if (!_maps.Contains(map))
            {
                _maps.Add(map);
                OnAddMap.OnNext(default);
            }
        }

        public void Remove(MapController map)
        {
            if (_maps.Remove(map))
                OnRemoveMap.OnNext(default);
        }

        public void Get(List<MapController> outList)
        {
            outList.Clear();
            outList.AddRange(_maps);
        }
    }
}