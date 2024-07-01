using UI.Service.Interface;

namespace UI.Service;

public class Throttle : IThrottle
{
    private readonly int _delay;
    private CancellationTokenSource _throttleTokenSource;

    public Throttle(int delay)
    {
        _delay = delay;
        _throttleTokenSource = new CancellationTokenSource();
    }

    public async Task RunAsync(Func<Task> action)
    {
        await _throttleTokenSource.CancelAsync();
        _throttleTokenSource.Dispose();

        _throttleTokenSource = new CancellationTokenSource();

        try
        {
            await Task.Delay(_delay, _throttleTokenSource.Token);
            await action();
        }
        catch (TaskCanceledException) {}
    }
}
