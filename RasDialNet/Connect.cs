using System;
using DotRas;

namespace RasDialNet
{
    class Connect : Command
    {
        private readonly RasDialOptions _options;

        public Connect(RasDialOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");
            _options = options;
        }

        public override void Execute()
        {
            var dialer = new RasDialer
                         {
                             AllowUseStoredCredentials = true,
                             EntryName                 = _options.ConnectionName,
                             PhoneBookPath             = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User)
                         };
            dialer.Dial();
        }
    }
}