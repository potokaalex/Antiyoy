using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    [ShowOdinSerializedPropertiesInInspector]
    public class SerializedMonoInstaller : MonoInstaller, ISerializationCallbackReceiver, ISupportsPrefabSerialization
    {
        [SerializeField, HideInInspector] private SerializationData _serializationData;

        SerializationData ISupportsPrefabSerialization.SerializationData
        {
            get => _serializationData;
            set => _serializationData = value;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() =>
            UnitySerializationUtility.DeserializeUnityObject(this, ref _serializationData);

        void ISerializationCallbackReceiver.OnBeforeSerialize() => UnitySerializationUtility.SerializeUnityObject(this, ref _serializationData);
    }
}