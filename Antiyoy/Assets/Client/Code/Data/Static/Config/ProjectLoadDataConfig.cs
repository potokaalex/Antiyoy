using System;
using ClientCode.Data.Progress.Project;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ClientCode.Data.Static.Config
{
    [CreateAssetMenu(menuName = "Configs/ProjectLoad", fileName = "ProjectLoadDataConfig", order = 0)]
    public class ProjectLoadDataConfig : SerializedScriptableObject
    {
        [NonSerialized, OdinSerialize] public ProjectLoadData Data;
    }
}