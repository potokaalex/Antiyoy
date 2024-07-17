using ClientCode.Infrastructure.States.Project;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Base;
using Zenject;

namespace ClientCode.UI.Buttons
{
    public class ProjectExitButton : ButtonBase
    {
        private IStateMachine _stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;

        private protected override void OnClick() => _stateMachine.SwitchTo<ProjectExitState>();
    }
}