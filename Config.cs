using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace SimpleOscBot
{
    /// <summary>
    /// Config file is loaded here
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Data model for config data, if a field is added to config, add it here to access it
        /// </summary>
        public class ConfigModel
        {
            public string BotToken { get; init; } = string.Empty;
            public ulong GuildId { get; init; } = 0;

            public string DefaultSendIp { get; init; } = string.Empty;
            public int DefaultSendPort { get; init; } = -1;

            public bool GlobalCommands { get; init; } = false;

            public List<ListenerModel> Listeners { get; init; } = new();

            public LoggingModel EnabledLogs { get; init; } = new();
        }

        public class ListenerModel
        {
            public string Name { get; init; } = string.Empty;
            public int Port { get; init; } = -1;
            public string Type { get; init; } = string.Empty;
            public List<object> Data { get; init; } = new();
        }

        public class LoggingModel
        {
            public bool Info { get; init; } = true;
            public bool Warning { get; init; } = true;
            public bool Error { get; init; } = true;
            public bool Log { get; init; } = true;
            public bool Debug { get; init; } = true;
        }

        public static string ConfigPath;
        public static string ResourcePath;
        public static ConfigModel Data { get; private set; }

        static Config()
        {
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory();

            ResourcePath = Path.GetFullPath(Path.Combine(assemblyDirectory, "config"));
            ConfigPath = Path.GetFullPath(Path.Combine(ResourcePath, "config.json"));

            try
            {
                if (!Directory.Exists(ResourcePath))
                    Directory.CreateDirectory(ResourcePath);

                string configData = File.ReadAllText(ConfigPath, Encoding.UTF8);
                Data = JsonConvert.DeserializeObject<ConfigModel>(configData) ?? new();
            }
            catch
            {
                Data = new();
            }
        }
    }
}
