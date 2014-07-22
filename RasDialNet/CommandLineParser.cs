using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RasDialNet
{
    public class CommandLineParser<T> 
        where T : new()
    {
        public T Parse(string[] args)
        {
            var options = new T();

            var argList = BuildArgList(args).ToList();
            var properties = options.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            var argsWithoutKeys = new List<Argument>();

            // Set all the named arguments and leave only unnamed ones
            foreach (var argument in argList)
            {
                if (argument.HasKey)
                {
                    var property = properties.SingleOrDefault(x => x.Name.StartsWith(argument.Key, StringComparison.InvariantCultureIgnoreCase));
                    SetPropertyValue(options, property, argument.Value);
                    properties.Remove(property);
                }
                else
                {
                    argsWithoutKeys.Add(argument);
                }
            }

            // Positional args now only need to be in the same *order*
            for (int i = 0; i < argsWithoutKeys.Count; i++)
            {
                if (i >= properties.Count)
                {
                    throw new ArgumentException(string.Format("No positional argument found for {0}.", argsWithoutKeys[i]));
                }

                SetPropertyValue(options, properties[i], argsWithoutKeys[i].Value);
            }

            return options;
        }

        private static void SetPropertyValue(T options, PropertyInfo property, string arg)
        {
            object typedArg = null;

            if (property.PropertyType == typeof(string))
            {
                typedArg = arg;
            }
            else if (property.PropertyType == typeof(DateTimeOffset))
            {
                typedArg = DateTimeOffset.Parse(arg);
            }
            else
            {
                typedArg = Convert.ChangeType(arg, property.PropertyType);
            }

            property.SetValue(options, typedArg);
        }

        private ArgumentAttribute GetArgAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(ArgumentAttribute))
                               .Cast<ArgumentAttribute>()
                               .SingleOrDefault();
        }

        private static IEnumerable<Argument> BuildArgList(IEnumerable<string> args)
        {
            string key = null;
            foreach (var arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    key = arg.Substring(1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(arg))
                    {
                        yield return new Argument { Key = key, Value = arg };
                        key = null;
                    }
                    else
                    {
                        yield return new Argument { Key = null, Value = arg };
                    }
                }
            }
        }
    }

    internal class ArgumentAttribute : Attribute
    {
        public string Key { get; private set; }
        public int Order { get; private set; }

        public ArgumentAttribute(int order, string key)
        {
            Order = order;
            Key = key;
        }
    }

    internal class Argument
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool HasKey { get { return !string.IsNullOrWhiteSpace(Key); }}
    }
}