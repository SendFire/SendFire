using SendFire.Common.Process;
using System.Collections.Generic;
using Xunit;
namespace SendFire.Common.Tests.Process
{
    public class CommandLineExecutionProviderTest
    {
        [Theory,
            MemberData(nameof(SystemCommandArguments))]
        public void ProcessCommand(string[] args)
        {
            var commandLineExecution = new CommandLineExecutionProvider(args);
            var output = commandLineExecution.ProcessCommand();
            Assert.NotEmpty(output);
        }

        public static IEnumerable<object[]> SystemCommandArguments()
        {
            var systemCommandArgument =new[]
            {
               new object[] { new string[] { "-command=set", "-arguments=HOME", "-s"} },
               new object[] { new string[] { "-command=echo", "-arguments=HOME", "-s"} }
            };
            return systemCommandArgument;
        }
    }
}
