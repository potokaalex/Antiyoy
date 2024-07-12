using System.Collections.Generic;
using ClientCode.Data.SceneData;
using ClientCode.Services;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States
{
    public class ProjectLoadState : IState
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private ProjectLoadDataProvider _loadDataProvider;

        public ProjectLoadState(IStaticDataProvider staticDataProvider) => _staticDataProvider = staticDataProvider;

        public void Enter()
        {
            var loadData = _loadDataProvider.Get();
            InitializeStaticData(loadData);
        }

        private void InitializeStaticData(ProjectLoadData projectLoadData)
        {
            _staticDataProvider.Initialize(new List<IStaticData>
            {
                projectLoadData.SceneConfig
            });
        }
    }
}