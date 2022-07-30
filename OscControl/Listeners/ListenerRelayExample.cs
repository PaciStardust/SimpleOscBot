using SimpleOscBot.Services;

namespace SimpleOscBot.OSCControl
{
    /// <summary>
    /// Example Listener that sends data to multiple ports
    /// </summary>
    public class ListenerRelayExample : OscListenerBase
    {
        //Array of ports to send to
        private readonly List<int> _outPorts = new();

        //Overriding data assignment
        protected override void AssignData(List<object> data)
        {
            foreach (var port in data)
            {
                _outPorts.Add(Convert.ToInt32(port));
            }
        }

        //Gets run whenever data is sent
        protected override Task HandleData(string address, params object[] args)
        {
            foreach (var outPort in _outPorts)
            {
                Logger.Log($"Forwarding data from \"{Name}\" port {Port} to {outPort}", Name);
                OSC.SendCustom(address, "127.0.0.1", outPort, args);
            }

            return Task.CompletedTask;
        }
    }
}
