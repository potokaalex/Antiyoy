namespace Code.Services.StateMachine
{
    public interface IStateMachine
    {
        public void SwitchTo<T>() where T : IState;
    }
}