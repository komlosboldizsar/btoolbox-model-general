namespace BToolbox.Logger
{
    public class FileLogger : IDisposable
    {

        public string FolderPath = @".\log";
        public LogMessageSeverity? MaxSeverity { get; init; } = LogMessageSeverity.Verbose;
        public string Tag = "log";
        public string Extension = "log";
        public string FilenameFormatter = "$tag$-$timestamp$.$extension$";

        private readonly string _filePath = null;
        private StreamWriter _streamWriter;

        public FileLogger()
        {
            string filename = FilenameFormatter
                .Replace("$tag$", Tag)
                .Replace("$timestamp$", DateTime.Now.ToString("yyyyMMdd-HHmmss"))
                .Replace("$extension$", Extension);
            _filePath = $"{FolderPath}{Path.DirectorySeparatorChar}{filename}";
            string directoryPath = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            FileStreamOptions streamWriterOptions = new()
            {
                Access = FileAccess.Write,
                Mode = FileMode.Create,
                Share = FileShare.Read
            };
            _streamWriter = new(_filePath, streamWriterOptions)
            {
                AutoFlush = true
            };
            LogDispatcher.NewLogMessage += newLogMessageHandler;
        }

        public void Dispose()
        {
            _streamWriter?.Dispose();
            _streamWriter = null;
        }

        private void newLogMessageHandler(DateTime timestamp, LogMessageSeverity severity, string message)
        {
            if ((MaxSeverity != null) && (severity > MaxSeverity))
                return;
            lock (_streamWriter)
            {
                _streamWriter.WriteLine($"[{timestamp:HH:mm:ss}][{convertTypeToString(severity)}] {message}");
            }
        }

        private static string convertTypeToString(LogMessageSeverity severity)
            => severity switch
            {
                LogMessageSeverity.Error => "ERROR",
                LogMessageSeverity.Warning => "WARN",
                LogMessageSeverity.Info => "INFO",
                LogMessageSeverity.Verbose => "VERBOSE",
                LogMessageSeverity.VerbosePlus => "VERBOSE+",
                _ => "?",
            };

    }
}
