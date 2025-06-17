using Client.Code.Services.StateMachineCode.State;
using ClientCode.Data.Progress.Project;
using ClientCode.Data.Static;
using ClientCode.Infrastructure.States.MainMenu;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.StaticDataProvider;
using ClientCode.Services.Updater;

namespace ClientCode.Infrastructure.States.Project
{
    public class ProjectLoadState : IStateSimple
    {
        private readonly IProjectSaveLoader _saveLoader;
        private readonly IProjectStateMachine _stateMachine;
        private readonly IStaticDataProvider _staticData;
        private readonly IUpdater _updater;

        public ProjectLoadState(IStaticDataProvider staticData, IProjectSaveLoader saveLoader, IProjectStateMachine stateMachine, IUpdater updater)
        {
            _staticData = staticData;
            _saveLoader = saveLoader;
            _stateMachine = stateMachine;
            _updater = updater;
        }

        public void Enter()
        {
            _saveLoader.Load(out var progress);
            InitializeStaticData(progress);
            //
            _updater.OnProjectExit += () => _stateMachine.SwitchTo<ProjectExitState>();
            _stateMachine.SwitchTo<MainMenuLoadState>();
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