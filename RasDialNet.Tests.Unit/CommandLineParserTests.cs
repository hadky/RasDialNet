using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RasDialNet.Tests.Unit
{
    [TestFixture]
    public class CommandLineParser1Tests
    {
        [Test]
        public void NoArgs_Creates_Empty_Options()
        {
            var parser = new CommandLineParser<OptionsWithOneArg>();
            var options = parser.Parse(new string[0]);
            
            Assert.That(options.Action, Is.Null);
        }

        [Test]
        public void SingleStringArg_with_no_key()
        {
            var parser = new CommandLineParser<OptionsWithOneArg>();
            var options = parser.Parse(new[] { "connect" });

            Assert.That(options.Action, Is.EqualTo("connect"));
        }

        [Test]
        public void TwoArgs_sets_two_properties()
        {
            var parser = new CommandLineParser<OptionsWithTwoArgs>();
            var options = parser.Parse(new[] { "connect", "myvpn" });

            Assert.That(options.Action, Is.EqualTo("connect"));
            Assert.That(options.ConnectionName, Is.EqualTo("myvpn"));
        }

        [Test]
        public void PositionalArgAndNamedArg_sets_two_properties()
        {
            var parser = new CommandLineParser<OptionsWithTwoArgs>();
            var options = parser.Parse(new[] { "connect", "-ConnectionName", "myvpn" });

            Assert.That(options.Action, Is.EqualTo("connect"));
            Assert.That(options.ConnectionName, Is.EqualTo("myvpn"));
        }

        [Test]
        public void NamedArgAndPositionalArg_sets_two_properties()
        {
            var parser = new CommandLineParser<OptionsWithTwoArgs>();
            var options = parser.Parse(new[] { "-ConnectionName", "myvpn", "connect" });

            Assert.That(options.Action, Is.EqualTo("connect"));
            Assert.That(options.ConnectionName, Is.EqualTo("myvpn"));
        }

        [Test]
        public void AbbreviatedNamedArgAndPositionalArg_sets_two_properties()
        {
            var parser = new CommandLineParser<OptionsWithTwoArgs>();
            var options = parser.Parse(new[] { "-c", "myvpn", "connect" });

            Assert.That(options.Action, Is.EqualTo("connect"));
            Assert.That(options.ConnectionName, Is.EqualTo("myvpn"));
        }

        [Test]
        public void NonStringOption_is_set()
        {
            var parser = new CommandLineParser<OptionsWithInt>();
            var options = parser.Parse(new[] { "123", "2014-07-22T10:20:00+01" });

            Assert.That(options.NumericArg, Is.EqualTo(123));
            Assert.That(options.DateTimeOffsetArg, Is.EqualTo(DateTimeOffset.Parse("2014-07-22T10:20:00+01")));
        }
    }

    public class OptionsWithInt
    {
        public int NumericArg { get; set; }
        public DateTimeOffset DateTimeOffsetArg { get; set; }
    }

    public class OptionsWithTwoArgs
    {
        public string Action { get; set; }
        public string ConnectionName { get; set; }
    }

    public class OptionsWithOneArg
    {
        public string Action { get; set; }
    }
}
