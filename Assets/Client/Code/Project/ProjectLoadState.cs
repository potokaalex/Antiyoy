using ClientCode.Data.Progress.Project;
using ClientCode.Data.Static;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.Project
{
    public class ProjectLoadState : IState
    {
        private readonly IProjectSaveLoader _saveLoader;
        private readonly IProjectStateMachine _stateMachine;
        private readonly IStaticDataProvider _staticData;

        public ProjectLoadState(IStaticDataProvider staticData, IProjectSaveLoader saveLoader, IProjectStateMachine stateMachine)
        {
            _staticData = staticData;
            _saveLoader = saveLoader;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _saveLoader.Load(out var progress);
            InitializeStaticData(progress);
            _stateMachine.SwitchTo<ProjectEnterSate>();
        }

        private void InitializeStaticData(ProjectProgressData progress)
        {
            var configs = progress.Load.Configs;
            var prefabs = new Prefabs
            {
                ProjectCanvasObject = configs.UI.ProjectCanvasObject,
                Buttons = configs.UI.Buttons,
                Windows = configs.UI.Windows,
                GridObject = configs.Gameplay.GridObject,
                EmptyTile = configs.Gameplay.EmptyTile,
                Tile = configs.Gameplay.Tile,
                CellDebug = configs.Gameplay.CellDebug
            };
            _staticData.Initialize(configs, prefabs);
        }
    }
}