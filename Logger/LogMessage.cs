﻿namespace BToolbox.Logger
{
    public record LogMessage(DateTime Timestamp, LogMessageSeverity Severity, string Message);
}
