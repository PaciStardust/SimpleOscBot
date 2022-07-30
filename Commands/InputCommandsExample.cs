using Discord.Interactions;
using SimpleOscBot.OSCControl;
using SimpleOscBot.Services;

namespace SimpleOscBot.Commands
{
    //Class must inherit from InteractionModuleBase to be loaded as command
    //"Group" signifies a group of commands
    //"EnabledInDm" activates usage in discord dms
    //Command names, group names and parameter names cannot be uppercase, all commands must be public
    [Group("input", "Various examples for input commands"), EnabledInDm(true)]
    public class InputCommandsExample : InteractionModuleBase
    {
        // We will be using this function to make sending data easier, the port is the same as ListenerRelayExample
        private static bool SendAction(string address, params object[] args)
            => OSC.SendCustom(address, "127.0.0.1", 9010, args);

        //Helper function for timed actions
        private static async Task SendActionTimed(string address, int time, int value = 1)
        {
            SendAction(address, value);
            await Task.Delay(time); //wait x ms
            SendAction(address, 0);
        }

        //Another helper function for replying with embeds
        private async Task RespondEmbed(string message)
            => await RespondAsync(embed: Embeds.Info("Player Input Control", message));
            

        //Slash command, these can basically take anything as parameter, we will be using async tasks for these
        [SlashCommand("jump", "Make player jump")]
        public async Task ControlJump()
        {
            await RespondEmbed("Jumped!");
            await SendActionTimed("/input/Jump", 25);
        }

        public enum MoveDirection
        {
            Forward,
            Backward,
            Left,
            Right
        }

        //Example: Enum parameter and a time value capped at 5000
        [SlashCommand("move", "Make player move")]
        public async Task ControlMove(MoveDirection direction, [MinValue(1), MaxValue(5000)] int time)
        {
            var address = "/input/Move" + direction.ToString();
            await RespondEmbed($"Moving {direction} for {time}ms!");
            await SendActionTimed(address, time);
        }

        public enum TurnDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        //Example: Enum parameter and a time value capped at 5000
        [SlashCommand("turn", "Make player turn")]
        public async Task ControlTurn(TurnDirection direction, [MinValue(1), MaxValue(5000)] int time)
        {
            var address = "/input/Look" + direction.ToString();
            await RespondEmbed($"Turning {direction} for {time}ms!");
            await SendActionTimed(address, time);
        }

        //If you assign a value to a variable in the function parameters it becomes optional
        //RequireOwner locks a command to only the bot owner
        [RequireOwner, SlashCommand("custom", "Make a custom input")]
        public async Task ControlCustom(string address, int value, [MinValue(0), MaxValue(5000)] int time = 0)
        {
            if (time > 0)
            {
                await RespondEmbed($"Executing \"{address}\" for {time}ms");
                await SendActionTimed(address, time, value);
                return;
            }

            bool success = SendAction(address, value);

            await RespondEmbed(success ? $"Executed \"{address}\"" : "There was an error in execution\nPlease check logs");
        }

        [SlashCommand("vrc-emote", "play an emote for x time")]
        public async Task ControlEmote([MinValue(0), MaxValue(99)] int emote, [MinValue(0), MaxValue(5000)] int time)
        {
            var address = "/avatar/parameters/VRCEmote";
            await RespondEmbed($"Doing emote {emote} for {time}ms!");
            await SendActionTimed(address, time, emote);
        }

        //This only works when a parameter called PlaySpeed exists
        [SlashCommand("vrc-speed", "set emote speed")]
        public async Task ControlSpeed([MinValue(0), MaxValue(1)] float speed)
        {
            var address = "/avatar/parameters/PlaySpeed";
            SendAction(address, speed);
            await RespondEmbed($"Setting emote speed to {speed}");
        }
    }
}
