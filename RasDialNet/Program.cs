using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasDialNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new CommandLineParser<RasDialOptions>();
            var options = parser.Parse(args);

            var commandType = typeof(Command).Assembly
                                             .GetTypes()
                                             .SingleOrDefault(x => x.Name.Equals(options.Action, StringComparison.InvariantCultureIgnoreCase));

            if (commandType == null)
            {
                throw new ArgumentException(string.Format("No action found for {0}.", options.Action));
            }

            var command = (Command)Activator.CreateInstance(commandType, new[] { options });
            command.Execute();
        }
    }
}
