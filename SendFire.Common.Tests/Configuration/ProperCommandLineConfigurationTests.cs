using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SendFire.Common.Configuration;
using SendFire.Common.ExtensionMethods;
using Xunit;

namespace SendFire.Common.Tests.Configuration
{
    /// <summary>
    /// I had to replace the standard .NET Core CommandLineConfigurationProvider and CommandLineConfigurationSource with 
    /// custom provider and source that normalize the behavior, may do a pull request on this sooner or later:
    /// 
    /// https://github.com/aspnet/Configuration/issues/507
    /// 
    /// </summary>
    public class ProperCommandLineConfigurationTests
    {
        [Theory,
            MemberData(nameof(ExpectedConversionsTests))]
        public void ProviderExpectedConversions(string[] argsGiven, Dictionary<string, string> switchMappings, Dictionary<string, string> expectedData)
        {
            //TODO - Proper .NET Core CommandLineConfigurationProvider has "strange" behavior, I copied and modified to expected behavior.
            var commandLineConfigurationProvider = new ProperCommandLineConfigurationProvider(argsGiven, switchMappings);
            commandLineConfigurationProvider.Load();
            foreach (var expected in expectedData)
            {
                Assert.True(commandLineConfigurationProvider.TryGet(expected.Key, out var value), $"Could not find '{expected.Key}' key");
                Assert.Equal(expected.Value, value, true);
            }
        }

        [Theory,
         MemberData(nameof(ExpectedConversionsTests))]
        public void BuilderExpectedConversions(string[] argsGiven, Dictionary<string, string> switchMappings, Dictionary<string, string> expectedData)
        {
            //TODO - Proper .NET Core CommandLineConfigurationProvider has "strange" behavior, I copied and modified to expected behavior.
            var configuration = new ConfigurationBuilder().AddProperCommandLine(argsGiven, switchMappings).Build();
            foreach (var expected in expectedData)
            {
                Assert.Equal(expected.Value, configuration[expected.Key], true);
            }
        }

        [Theory,
         MemberData(nameof(ExpectedConversionsTests))]
        public void BuilderReturnsNullForParameterNotPassed(string[] argsGiven, Dictionary<string, string> switchMappings, Dictionary<string, string> expectedData)
        {
            //TODO - Proper .NET Core CommandLineConfigurationProvider has "strange" behavior, I copied and modified to expected behavior.
            var configuration = new ConfigurationBuilder().AddProperCommandLine(argsGiven, switchMappings).Build();
            var dummy = expectedData;
            Assert.Null(configuration["IdontExist"]);
        }

        public static IEnumerable<object[]> ExpectedConversionsTests => new[]
        {
            new object[] {
                new [] { "-c", "--help", "--someotherflag=false", @"--file=""C:\Output\Wherever I May Go""" },
                new Dictionary<string, string>() { { "-c" , "command"}, },
                new Dictionary<string, string>()
                {
                    { "command", "true"},
                    { "help", "tRue" },
                    { "someotherflag", "False" },
                    { "file", "C:\\Output\\Wherever I May Go" }
                }
            },
            new object[] {
                new [] { "/command", "--help", @"--file=C:\Output\WhereverIMayGo" },
                null,
                new Dictionary<string, string>()
                {
                    { "command", "TRUE"},
                    { "help", "TRue" },
                    { "file", "C:\\Output\\WhereverIMayGo" }
                }
            }
        };
    }
}
