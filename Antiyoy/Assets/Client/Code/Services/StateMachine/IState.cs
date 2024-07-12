namespace Code.Services.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit() {}
    }
}