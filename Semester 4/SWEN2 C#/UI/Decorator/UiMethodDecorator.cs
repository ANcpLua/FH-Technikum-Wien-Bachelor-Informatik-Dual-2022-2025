using System.Diagnostics;
using System.Reflection;
using MethodDecorator.Fody.Interfaces;
using Serilog;
using UI.Service.Interface;
using UI.ViewModel.Base;
using ILogger=Serilog.ILogger;

namespace UI.Decorator;

[AttributeUsage(
AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Assembly | AttributeTargets.Module
)]
public class UiMethodDecorator : Attribute, IMethodDecorator
{
    private object[] _args = [];
    private ILogger _logger = Log.Logger;
    private string _methodName = string.Empty;
    private Stopwatch _stopwatch = new();
    private IToastServiceWrapper? _toastService;

    public void Init(object instance, MethodBase method, object[] args)
    {
        _logger = Log.Logger;
        _methodName = $"{method.DeclaringType?.FullName}.{method.Name}";
        _args = args;
        _logger.Information(
        "Entering {MethodName} with arguments: {@Arguments}",
        _methodName,
        args
        );
        _stopwatch = Stopwatch.StartNew();

        if (instance is BaseViewModel viewModel)
        {
            _toastService = viewModel.ToastServiceWrapper;
        }
    }

    public void OnEntry() {}

    public void OnExit()
    {
        _stopwatch.Stop();
        _logger.Information(
        "Exiting {MethodName} after {Duration}ms",
        _methodName,
        _stopwatch.ElapsedMilliseconds
        );
    }

    public void OnException(Exception exception)
    {
        _stopwatch.Stop();
        _logger.Error(
        exception,
        "Exception in {MethodName} with arguments: {@Arguments} after {Duration}ms",
        _methodName,
        _args,
        _stopwatch.ElapsedMilliseconds
        );
        _toastService?.ShowError($"An error occurred in {_methodName}: {exception.Message}");
    }
}