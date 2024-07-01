using UI.Service.Interface;
using ILogger=Serilog.ILogger;

namespace UI.Decorator;

public class TryCatchToastWrapper
{
    private readonly ILogger _logger;
    private readonly IToastServiceWrapper _toastServiceWrapper;

    public TryCatchToastWrapper(IToastServiceWrapper toastServiceWrapper, ILogger logger)
    {
        _toastServiceWrapper = toastServiceWrapper;
        _logger = logger;
    }

    public async Task<T?> ExecuteAsync<T>(
        Func<Task<T>> action,
        string errorMessage,
        Action<Exception>? errorHandler = null
    )
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            errorHandler?.Invoke(ex);
            _logger.Error(ex, errorMessage);
            _toastServiceWrapper.ShowError($"{errorMessage}: {ex.Message}");
            return default;
        }
    }

    public async Task ExecuteAsync(
        Func<Task> action,
        string errorMessage,
        Action<Exception>? errorHandler = null
    )
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            errorHandler?.Invoke(ex);
            _logger.Error(ex, errorMessage);
            _toastServiceWrapper.ShowError($"{errorMessage}: {ex.Message}");
        }
    }
}
