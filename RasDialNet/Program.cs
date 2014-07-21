using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotRas;

namespace RasDialNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var dialer = new RasDialer();
            dialer.AllowUseStoredCredentials = true;
            dialer.EntryName = args[0];
            dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
            var handle = dialer.Dial();
        }
    }
}
