using SendFire.Common.Process;
using System.Collections.Generic;
using Xunit;
namespace SendFire.Common.Tests.Process
{
    public class CommandLineExecutionProviderTest
    {
        public static string today = System.DateTime.Now.ToShortDateString();
        [Theory,
            MemberData(nameof(SystemCommandArguments))]
        public void ProcessCommand(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
          //  Assert.Contains("JAVAHOME", output);
        }
        [Theory,
           InlineData(new string[] { "echo %HOME%", "set JAVAHOME=\"C:\\Temp\"", "echo %JAVAHOME%" }, true)]
        public void ProcessSysCommand(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
            Assert.Contains("JAVAHOME", output);
        }
        [Theory,
           InlineData(new string[] { "mkdir c:\\temp3" ,"echo eun555" },true),
           InlineData(new string[] { "mkdir c:\\temp5","cd c:\\temp5", "mkdir eun555", "dir" }, true)]
        public void ProcessSysCommand2(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
            Assert.Contains("eun555", output);
        }
        [Theory,
           InlineData(new string[] { "cmd /c dir c:\\" }, false)]
        public void ProcessMultipleCommands(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
            Assert.Contains("Program", output);
         }
        [Theory,
        InlineData(new string[] { "cmd /c cd c:\\temp" ,"cmd /c mkdir c:\\temp\\blob", "cmd /c dir c:\\temp" }, false)]
        public void ProcessMultipleCommands2(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
            Assert.Contains("temp",output);
            Assert.Contains("blob", output);
        }

        public static IEnumerable<object[]> SystemCommandArguments()
        {
            var systemCommandArgument =new[]
            {
               new object[] { new string[] { "dir c:\\"}, true },
               new object[] { new string[] { "echo %HOME%", "set JAVAHOME=\"C:\\Temp\"", "echo %JAVAHOME%"}, true }
            };
            return systemCommandArgument;
        }
    }
}
