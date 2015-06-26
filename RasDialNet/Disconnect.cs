using System;
using DotRas;

namespace RasDialNet
{
    class Disconnect : Command
    {
        private readonly RasDialOptions _options;
        private readonly ILogger _logger;

        public Disconnect(RasDialOptions options, ILogger logger)
        {
            if (options == null) throw new ArgumentNullException("options");
            if (logger == null) throw new ArgumentNullException("logger");
            
            _options = options;
            _logger = logger;
        }

        public override void Execute()
        {
            _logger.Info(() => string.Format("Disconnecting {0}...", _options.ConnectionName));

            var connections = RasConnection.GetActiveConnections();
            foreach (var connection in connections)
            {
                if (connection.EntryName.Equals(_options.ConnectionName, StringComparison.InvariantCultureIgnoreCase))
                {
                    connection.HangUp(true);
                    _logger.Info(() => "Disconnected.");
                    return;
                }
            }

            _logger.Warning(() => string.Format("No connection found for '{0}', No connection disconnected.", _options.ConnectionName));
        }
    }
}