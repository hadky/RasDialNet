using System;
using DotRas;

namespace RasDialNet
{
    class Connect : Command
    {
        private readonly RasDialOptions _options;
        private readonly ILogger _logger;

        public Connect(RasDialOptions options, ILogger logger)
        {
            if (options == null) throw new ArgumentNullException("options");
            if (logger == null) throw new ArgumentNullException("logger");

            _options = options;
            _logger = logger;
        }

        public override void Execute()
        {
            _logger.Info(() => string.Format("Dialling connection '{0}'...", _options.ConnectionName));

            var dialer = new RasDialer
                         {
                             AllowUseStoredCredentials = true,
                             EntryName                 = _options.ConnectionName,
                             PhoneBookPath             = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User)
                         };
            dialer.Dial();

            _logger.Info(() => "Connected.");
        }
    }
}