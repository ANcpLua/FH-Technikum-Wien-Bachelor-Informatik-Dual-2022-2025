namespace UI.Service.Interface;

public interface IThrottle
{
    Task RunAsync(Func<Task> action);
}
