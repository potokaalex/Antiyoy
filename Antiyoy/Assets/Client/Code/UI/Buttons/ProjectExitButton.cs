using ClientCode.Infrastructure.States.Project;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Base;
using Zenject;

namespace ClientCode.UI.Buttons
{
    public class ProjectExitButton : ButtonBase
    {
        private IProjectStateMachine _stateMachine;

        [Inject]
        public void Construct(IProjectStateMachine stateMachine) => _stateMachine = stateMachine;

        private protected override void OnClick() => _stateMachine.SwitchTo<ProjectExitState>();
    }
}