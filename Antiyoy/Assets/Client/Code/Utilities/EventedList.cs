using System;
using System.Collections;
using System.Collections.Generic;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace ClientCode.Utilities
{
    public class EventedList<T> : IEnumerable<T>
    {
        private readonly List<T> _list;
            
        public EventedList(List<T> list) => _list = list;

        public event Action<T> OnAdded;
        
        public event Action<T> OnRemoved;

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public int Count => _list.Count;

        public void Add(T item)
        {
            _list.Add(item);
            OnAdded?.Invoke(item);
        }

        public bool Remove(T item)
        {
            if (!_list.Remove(item)) 
                return false;
            
            OnRemoved?.Invoke(item);
            return true;
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}