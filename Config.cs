using Newtonsoft.Json;
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
            public string BotToken { get; init; }
            public ulong GuildId { get; init; }

            public string DefaultSendIp { get; init; }
            public int DefaultSendPort { get; init; }

            public bool GlobalCommands { get; init; }

            public List<ListenerModel> Listeners { get; init; }
        }

        public class ListenerModel
        {
            public string Name { get; init; }
            public int Port { get; init; }
            public string Type { get; init; }
            public List<object> Data { get; init; }
        }

        public static string ConfigPath = Path.GetFullPath(@"../../../Resources/config.json");
        public static ConfigModel Data { get; private set; }

        static Config()
        {
            string configData = File.ReadAllText(ConfigPath, Encoding.UTF8);
            Data = JsonConvert.DeserializeObject<ConfigModel>(configData);
        }
    }
}
