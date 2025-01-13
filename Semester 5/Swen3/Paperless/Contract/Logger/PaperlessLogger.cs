using System;
using Microsoft.Extensions.Logging;

namespace Contract.Logger;

public class PaperlessLogger : IPaperlessLogger
{
    private readonly ILogger _logger;
    private readonly string _component;
    private readonly string _environment;

    public PaperlessLogger(ILogger logger, string component, string environment)
    {
        _logger = logger;
        _component = component;
        _environment = environment;
    }

    public void LogOperation(string category, string operation, string details)
        => _logger.LogInformation("[{Environment}][{Component}][{Category}] {Operation} - {Details}", 
            _environment, _component, category, operation, details);

    public void LogError(string category, string operation, string details, Exception ex)
        => _logger.LogError(ex, "[{Environment}][{Component}][{Category}] {Operation} - {Details}", 
            _environment, _component, category, operation, details);
}