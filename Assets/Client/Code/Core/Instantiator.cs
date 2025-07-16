using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Client.Code.Core
{
    public class Instantiator
    {
        private readonly IInstantiator _instantiator;

        public Instantiator(IInstantiator instantiator) => _instantiator = instantiator;

        public T Instantiate<T>(object[] args = null)
        {
            args ??= Array.Empty<object>();
            var instance = _instantiator.Instantiate<T>(args);
            return instance;
        }

        public T InstantiateForComponent<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true, bool isActive = true) =>
            (T)InstantiateForComponent(typeof(T), prefab, position, rotation, parent, worldPositionStays, isActive);

        public T InstantiateForComponent<T>(GameObject prefab, Transform parent = null, bool worldPositionStays = true, bool isActive = true) =>
            (T)InstantiateForComponent(typeof(T), prefab, parent, worldPositionStays, isActive);

        public object InstantiateForComponent(Type type, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true, bool isActive = true)
        {
            var instance = InstantiatePrefab(prefab, position, rotation, parent, worldPositionStays, isActive);
            return instance.GetComponentInChildren(type);
        }

        public object InstantiateForComponent(Type type, GameObject prefab, Transform parent = null,
            bool worldPositionStays = true, bool isActive = true)
        {
            var t = prefab.transform;
            return InstantiateForComponent(type, prefab, t.position, t.rotation, parent, worldPositionStays, isActive);
        }

        public void Destroy(GameObject go) => Object.Destroy(go);

        private GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true, bool isActive = true)
        {
            var prefabDefaultActive = prefab.gameObject.activeSelf;
            prefab.SetActive(isActive);
            var instance = _instantiator.InstantiatePrefab(prefab);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.transform.SetParent(parent, worldPositionStays);
            prefab.SetActive(prefabDefaultActive);
            instance.SetActive(isActive);
            return instance;
        }
    }
}