namespace ClientCode.Services.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit() {}
    }
}