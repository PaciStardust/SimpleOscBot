using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SimpleOscBot.Services;
using SimpleOscBot.OSCControl;

namespace SimpleOscBot
{
    /// <summary>
    /// Main class to start the program
    /// </summary>
    public static class Program
    {
        private static readonly DiscordSocketClient _client = new();
        private static readonly IServiceProvider _services = ConfigureServices();

        /// <summary>
        /// Start of program, initializes everything
        /// </summary>
        public static async Task Main(string[] args)
        {
            //Starting up interactions (Slash commands, buttons, etc)
            await _services.GetRequiredService<InteractionHandler>().InitializeAsync();

            //Adding logging function and ready action
            _client.Log += Log;
            _client.Ready += ReadyAsync;

            //Login
            await _client.LoginAsync(TokenType.Bot, Config.Data.BotToken);
            await _client.StartAsync();
            await _client.SetGameAsync("SimpleOscBot by Paci Stardust", type: ActivityType.Playing);

            //Start all listeners
            OSC.InitializeListeners();

            //Postpone closing of program indefinetely
            await Task.Delay(-1);
        }

        /// <summary>
        /// Registers all commands in discord
        /// </summary>
        private static async Task ReadyAsync()
        {
            var interactService = _services.GetRequiredService<InteractionService>();
            await interactService.RegisterCommandsToGuildAsync(Config.Data.GuildId);

            if (Config.Data.GlobalCommands)
            {
                //This registers all commands globally, this takes 30 minutes and should only be done once you have set up everything
                await interactService.RegisterCommandsGloballyAsync(true);
            }
        }

        /// <summary>
        /// Sends discords logs to the logger class
        /// </summary>
        /// <param name="msg"></param>
        private static Task Log(LogMessage msg)
        {
            Logger.Log(msg);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Loading in all services
        /// </summary>
        private static ServiceProvider ConfigureServices() => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton<InteractionService>()
            .AddSingleton<InteractionHandler>()
            .BuildServiceProvider();
    }
}