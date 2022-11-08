using CoreOSC;
using SimpleOscBot.Services;

namespace SimpleOscBot.OSCControl
{
    /// <summary>
    /// Base for the OSC-Listener
    /// </summary>
    public abstract class OscListenerBase
    {
        public int Port { get; private set; } = -1;
        public string Name { get; private set; } = string.Empty;
        private bool _started = false;

        public OscListenerBase() {}

        /// <summary>
        /// Starts the listener, should never be called manually
        /// </summary>
        public void Start(string name, int port, List<object> data)
        {
            if (_started)
            {
                Logger.Warning("OSC-Listeners can only be started once", name);
                return;
            }
            _started = true;

            Name = name;
            Port = port;
            AssignData(data);

            void callback(OscPacket packet)
            {
                var message = (OscMessage)packet;

                if (message == null)
                {
                    Logger.Warning("Received an empty packet, this appears to happen when sending bool values, if that is the case please switch to 0 and 1", Name);
                    return;
                }

                Logger.Log($"Packet has been received on \"{name}\" port {port} ({message.Address})", Name);
                HandleData(message.Address, message.Arguments.ToArray());
            }

            try
            {
                var listener = new UDPListener(port, callback);
                Logger.Info($"OSC-Listener \"{name}\" ({GetType().Name}) is now listening on port {port}", "OSCInit");
            }
            catch (Exception e)
            {
                Logger.Error(e, "OSCInit");
            }
        } 

        /// <summary>
        /// Extra data from config file to import
        /// </summary>
        /// <param name="data">Json data</param>
        protected virtual void AssignData(List<object> data) { }

        /// <summary>
        /// Function to handle incoming data
        /// </summary>
        /// <param name="address">OSC Address</param>
        /// <param name="args">OSC Variables</param>
        /// <returns></returns>
        protected abstract Task HandleData(string address, params object[] args);
    }
}
