using System;
using Leopotam.EcsLite;

namespace Plugins.EcsLite
{
    public static class EcsLiteExtensions
    {
        public static bool Has<T>(this EcsPool<T> pool, EcsFilter filter, Predicate<T> predicate) where T : struct
        {
            foreach (var entity in filter)
                if (predicate.Invoke(pool.Get(entity)))
                    return true;

            return false;
        }

        public static int Find<T>(this EcsPool<T> pool, EcsFilter filter, Predicate<T> predicate) where T : struct
        {
            foreach (var entity in filter)
                if (predicate.Invoke(pool.Get(entity)))
                    return entity;

            throw new Exception("It is impossible to find an entity!");
        }

        public static ref T GetOrAdd<T>(this EcsPool<T> pool, int entity) where T : unmanaged
        {
            if (pool.Has(entity))
                return ref pool.Get(entity);

            return ref pool.Add(entity);
        }
    }
}