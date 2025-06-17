namespace ClientCode.Infrastructure.States.MapEditor
{
    public interface IProjectStateMachine
    {
        void SwitchTo<T>();
    }
}