using System;
using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Project;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ClientCode.Data.Static
{
    [CreateAssetMenu(menuName = "Configs/ProjectLoad", fileName = "ProjectLoadDataConfig", order = 0)]
    public class ProjectLoadDataConfig : SerializedScriptableObject
    {
        [NonSerialized, OdinSerialize] public ProjectLoadData Data;
    }
}