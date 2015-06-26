using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RasDialNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();

            var command = GetCommand(args, logger);

            if (command == null) return;

            ExecuteCommand(command, logger);
        }

        private static void ExecuteCommand(Command command, ILogger logger)
        {
            try
            {
                if (command == null) throw new ArgumentNullException("command");
                command.Execute();
                logger.Info(() => "Completed, closing...");
                Thread.Sleep(2000);
            }
            catch (Exception e)
            {
                logger.Error(string.Format("Unable to run command '{0}'", command.GetType()), e);
                Console.WriteLine("Press ENTER to continue...");
                Console.ReadLine();
            }
        }

        private static Command GetCommand(string[] args, ILogger logger)
        {
            try
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

                return (Command)Activator.CreateInstance(commandType, new object[] { options, logger });
            }
            catch (Exception e)
            {
                logger.Error("Failed to find command.", e);
            }

            return null;
        }
    }
}
