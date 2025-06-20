using UnityEngine;
using Zenject;

namespace ClientCode.Utilities.Extensions
{
    public static class ZenjectExtensions
    {
        public static T InstantiateMonoBehaviour<T>(this IInstantiator instantiator, T prefab, Transform parent = null,
            bool instantiateInWorldSpace = false, params object[] args) where T : MonoBehaviour
        {
            var instance = instantiator.InstantiatePrefabForComponent<T>(prefab.gameObject, args);

            if (parent != null)
                instance.transform.SetParent(parent, instantiateInWorldSpace);
            return instance;
        }
    }
}