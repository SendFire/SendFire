using SendFire.Common.Process;
using System;
using System.Collections.Generic;
using Xunit;
namespace SendFire.Common.Tests.Process
{
    public class CommandLineExecutionProviderTest
    {
        public static string today = System.DateTime.Now.ToShortDateString();
        [Theory,
            MemberData(nameof(SystemCommandArguments))]
        public void ProcessCommandAsBatch(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
        }
        [Theory,
           InlineData(new string[] { "echo %HOME%", "set JAVAHOME=\"C:\\Temp\"", "echo %JAVAHOME%" }, true)]
        public void ProcessCommandAsBatchForSysEnvironment(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
            Assert.Contains("JAVAHOME", output);
        }
        [Theory,
           InlineData(new string[] { "mkdir c:\\temp3" ,"echo eun555" },true),
           InlineData(new string[] { "mkdir c:\\temp5","cd c:\\temp5", "mkdir eun555", "dir" }, true)]
        public void ProcessCommandAsBatch2(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
            Assert.Contains("eun555", output);
        }
        [Theory,
           InlineData(new string[] { "cmd /c dir c:\\" }, false)]
        public void ProcessCommands(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch);
            Assert.NotEmpty(output);
            Assert.Contains("Program", output);
         }
        [Theory,
        InlineData(new string[] { "cmd /c cd c:\\temp" ,"cmd /c mkdir c:\\temp\\blob", "cmd /c dir c:\\temp" }, false)]
        public void ProcessCommands2(string[] commands, bool runAsBatch)
        {
            var timeoutMs = 30000;
            var commandLineExecution = new CommandLineExecutionProvider();
            var output = commandLineExecution.ProcessCommands(commands, runAsBatch, timeoutMs);
            Assert.NotEmpty(output);
            Assert.Contains("temp",output);
            Assert.Contains("blob", output);
        }
        [Theory,
          InlineData(new string[] { "cd c:\\temp", "pause", "dir c:\\temp" }, true)]
        public void ProcessCommandAsBatchForTimeoutTest(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var timeoutMs = 1000;
            Exception ex =Assert.Throws<TimeoutException>(()=> commandLineExecution.ProcessCommands(commands, runAsBatch, timeoutMs));
            Assert.Equal($"Process did not finish in {timeoutMs} ms.", ex.Message);
        }
        [Theory,
          InlineData(new string[] { "cmd /c cd c:\\temp", "cmd pause" }, false)]
        public void ProcessCommandForTimeoutTest(string[] commands, bool runAsBatch)
        {
            var commandLineExecution = new CommandLineExecutionProvider();
            var timeoutMs = 5000;
            Exception ex = Assert.Throws<TimeoutException>(() => commandLineExecution.ProcessCommands(commands, runAsBatch, timeoutMs));
            Assert.Equal($"Process did not finish in {timeoutMs} ms.", ex.Message);
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
