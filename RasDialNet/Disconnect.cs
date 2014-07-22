using System;
using DotRas;

namespace RasDialNet
{
    class Disconnect : Command
    {
        private readonly RasDialOptions _options;

        public Disconnect(RasDialOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");
            _options = options;
        }

        public override void Execute()
        {
            var connections = RasConnection.GetActiveConnections();
            foreach (var connection in connections)
            {
                if (connection.EntryName.Equals(_options.ConnectionName, StringComparison.InvariantCultureIgnoreCase))
                {
                    connection.HangUp(true);
                }
            }
        }
    }
}