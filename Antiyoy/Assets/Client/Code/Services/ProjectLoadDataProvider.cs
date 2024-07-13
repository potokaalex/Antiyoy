using System;
using ClientCode.Data.Static;
using UnityEngine;

namespace ClientCode.Services
{
    [Serializable]
    public class ProjectLoadDataProvider
    {
        [SerializeField] private ProjectLoadData _data;
        
        public ProjectLoadData Get() => _data;
    }
}