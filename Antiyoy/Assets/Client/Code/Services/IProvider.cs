namespace Client.Code.Services
{
    public interface IProvider<out T>
    {
        T Value { get; }
    }
}