using SimpleOscBot.Services;

namespace SimpleOscBot.OSCControl
{
    /// <summary>
    /// Example listener for debugging
    /// </summary>
    public class ListenerDebugExample : OscListenerBase
    {
        //Function override
        protected override Task HandleData(string address, params object[] args)
        {
            var argsInfo = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                var argType = args[i].GetType();
                var argValue = args[i].ToString();

                argsInfo.Add($"{i}: {argValue} ({argType.Name})");
            }

            var message = $"Data for address \"{address}\" - " + string.Join(", ", argsInfo);
            Logger.Debug(message, "OSCDebug");

            return Task.CompletedTask;
        }
    }
}
