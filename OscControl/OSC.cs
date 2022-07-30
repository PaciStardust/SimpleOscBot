using SharpOSC;
using SimpleOscBot.Services;

namespace SimpleOscBot.OSCControl
{
    /// <summary>
    /// Static class for osc-related things
    /// </summary>
    public static class OSC
    {
        private static bool _hasInitializedListeners = false;
        private static List<OscListenerBase> _listeners = new();

        /// <summary>
        /// Loads up an instance of any class that inherits from OscListenerBase
        /// </summary>
        public static void InitializeListeners()
        {
            if (_hasInitializedListeners)
            {
                Logger.Warning("OSC-Listeners cannot be initialized twice!", "OSCInit");
                return;
            }
            _hasInitializedListeners = true;

            var assembly = typeof(OSC).Assembly;
            foreach (var listener in Config.Data.Listeners)
            {
                try
                {
                    OscListenerBase lBase = (OscListenerBase)assembly.CreateInstance(listener.Type);
                    _listeners.Add(lBase);
                    lBase.Start(listener.Name, listener.Port, listener.Data ?? new());
                }
                catch (Exception e)
                {
                    Logger.Error(e, "OSCInit");
                }
            }
        }

        /// <summary>
        /// Send OSC Data with a non-default IP and Port
        /// </summary>
        /// <param name="address">OSC Address to send data to</param>
        /// <param name="ip">IP to send data to</param>
        /// <param name="port">Port to send data to</param>
        /// <param name="args">Variables to send</param>
        /// <returns>True on success</returns>
        public static bool SendCustom(string address, string ip, int port, params object[] args)
        {
            var message = new OscMessage(address, args);
            var sender = new UDPSender(ip, port);
            try
            {
                sender.Send(message);
                Logger.Log($"Successfully sent data to {ip}:{port} ({address})", "OSCSender");
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e, "OSCSender");
                return false;
            }
        }

        /// <summary>
        /// Send OSC Data to default IP and Port
        /// </summary>
        /// <param name="address">OSC Address to send data to</param>
        /// <param name="args">Variables to send</param>
        public static bool Send(string address, params object[] args)
            => SendCustom(address, Config.Data.DefaultSendIp, Config.Data.DefaultSendPort, args);

        //Utility for splitting address
        public static string[] SplitAddress(string address)
            => address[1..].Split('/');
    }
}
