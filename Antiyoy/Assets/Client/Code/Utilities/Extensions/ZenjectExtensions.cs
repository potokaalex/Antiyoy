using UnityEngine;
using Zenject;

namespace ClientCode.Utilities.Extensions
{
    public static class ZenjectExtensions
    {
        public static T InstantiateMonoBehaviour<T>(this IInstantiator instantiator, T prefab, Transform parent,
            bool instantiateInWorldSpace = false) where T : MonoBehaviour
        {
            var instance = instantiator.InstantiatePrefab(prefab.gameObject).GetComponent<T>();
            instance.transform.SetParent(parent, instantiateInWorldSpace);
            return instance;
        }
    }
}