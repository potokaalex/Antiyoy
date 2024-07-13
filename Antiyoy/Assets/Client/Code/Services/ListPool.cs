using System.Collections.Generic;

namespace ClientCode.Services
{
    public static class ListPool<T>
    {
        private static readonly List<List<T>> _freeItems = new();

        public static List<T> Get(int capacity)
        {
            foreach (var item in _freeItems)
            {
                if (item.Capacity >= capacity)
                {
                    _freeItems.Remove(item);
                    return item;
                }
            }

            return new List<T>(capacity);
        }

        public static List<T> Get()
        {
            if (_freeItems.Count == 0)
                return new List<T>();

            var lastIndex = _freeItems.Count - 1;
            var item = _freeItems[lastIndex];
            _freeItems.RemoveAt(lastIndex);
            return item;
        }

        public static void Release(List<T> item)
        {
            item.Clear();
            _freeItems.Add(item);
        }
    }
}