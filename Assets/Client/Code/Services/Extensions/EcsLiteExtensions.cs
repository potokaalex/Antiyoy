using System;
using Leopotam.EcsLite;

namespace Client.Code.Services.Extensions
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

        public static ref T GetOrAdd<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
                return ref pool.Get(entity);

            return ref pool.Add(entity);
        }

        public static int Get<T>(this EcsFilter filter, EcsPool<T> pool, Predicate<T> predicate) where T : struct
        {
            foreach (var entity in filter)
                if (predicate.Invoke(pool.Get(entity)))
                    return entity;

            throw new Exception("It is impossible to find an entity with this predicate!");
        }

        public static ref T Get<T>(this EcsPool<T> pool, EcsFilter filter, Predicate<T> predicate) where T : struct
        {
            foreach (var entity in filter)
            {
                ref var component = ref pool.Get(entity);

                if (predicate.Invoke(component))
                    return ref component;
            }

            throw new Exception("It is impossible to find an entity with this predicate!");
        }

        public static bool TryGet<T>(this EcsPool<T> pool, EcsFilter filter, Predicate<T> predicate, out T component) where T : struct
        {
            foreach (var entity in filter)
            {
                ref var c = ref pool.Get(entity);

                if (predicate.Invoke(c))
                {
                    component = c;
                    return true;
                }
            }

            component = default;
            return false;
        }
    }
}