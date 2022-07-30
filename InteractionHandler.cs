using Discord.Interactions;
using Discord.WebSocket;
using SimpleOscBot.Services;

namespace SimpleOscBot
{
    /// <summary>
    /// Handles all interactions (Slash commands, buttons, etc.)
    /// </summary>
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _intService;
        private readonly IServiceProvider _services;

        public InteractionHandler(DiscordSocketClient client, InteractionService intService, IServiceProvider services)
        {
            _client = client;
            _intService = intService;
            _services = services;

            _client.InteractionCreated += HandleInteraction;
        }

        /// <summary>
        /// Loading in all slash commands from assembly
        /// </summary>
        public async Task InitializeAsync()
        {
            await _intService.AddModulesAsync(GetType().Assembly, _services);
        }

        /// <summary>
        /// Executes received interactions
        /// </summary>
        /// <param name="interaction">Interaction to be executed</param>

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            var context = new SocketInteractionContext(_client, interaction);
            var result = await _intService.ExecuteCommandAsync(context, _services);

            await HandleInteractionErrors(result, context);
        }

        /// <summary>
        /// Logs success of executed interaction
        /// </summary>
        /// <param name="result">Result of the interaction</param>
        /// <param name="context">Context of the interaction</param>
        /// <returns></returns>
        private static async Task HandleInteractionErrors(IResult result, SocketInteractionContext context)
        {
            Logger.Log($"Executed command {context.Interaction.Id} for user {context.User.Username}({context.User.Id})", "IntHandler");

            if (result.IsSuccess) return;

            await context.Interaction.RespondAsync(embed: Embeds.Error(result.Error.ToString(), result.ErrorReason));
        }
    }
}
