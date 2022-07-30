using Discord;

namespace SimpleOscBot.Services
{
    /// <summary>
    /// Class for logging of information in console
    /// </summary>
    public static class Logger
    {
        public static void Log(LogMessage message)
        {
            var bufferColor = Console.ForegroundColor;

            Console.ForegroundColor = GetLogColor(message.Severity);
            Console.WriteLine(message.ToString());
            Console.ForegroundColor = bufferColor;
        }
        public static void Log(string message, string source, LogSeverity severity = LogSeverity.Verbose)
            => Log(new(severity, source, message));
        public static void Error(string message, string source)
            => Log(message, source, LogSeverity.Error);
        public static void Error(string type, string message, string source, string trace = "unspecified location")
           => Error($"A {type} has occured: {message} at {trace}", source);
        public static void Error(Exception error, string source)
            => Error(error.GetType().ToString(), error.Message, source, error.StackTrace ?? "unspecified location");
        public static void Info(string message, string source)
            =>Log (message, source, LogSeverity.Info);
        public static void Warning(string message, string source)
            => Log(message, source, LogSeverity.Warning);
        public static void Debug(string message, string source)
            => Log(message, source, LogSeverity.Debug);

        private static ConsoleColor GetLogColor(LogSeverity severity) => severity switch
        {
            LogSeverity.Error => ConsoleColor.Red,
            LogSeverity.Critical => ConsoleColor.DarkRed,
            LogSeverity.Warning => ConsoleColor.Yellow,
            LogSeverity.Verbose => ConsoleColor.DarkGray,
            LogSeverity.Info => ConsoleColor.White,
            LogSeverity.Debug => ConsoleColor.DarkBlue,
            _ => ConsoleColor.White
        };
    }
}
