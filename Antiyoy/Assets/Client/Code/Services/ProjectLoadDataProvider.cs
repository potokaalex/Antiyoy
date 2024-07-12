using System;
using ClientCode.Data.SceneData;
using UnityEngine;

namespace ClientCode.Services
{
    [Serializable]
    public class ProjectLoadDataProvider
    {
        [SerializeField] private ProjectLoadData _data;
        
        public ProjectLoadData Get() => _data;
    }
    
    //унимерсальный data-provider
}