using System;

namespace Contract.Logger;

public interface IPaperlessLogger
{
    void LogOperation(string category, string operation, string details);
    void LogError(string category, string operation, string details, Exception ex);
}