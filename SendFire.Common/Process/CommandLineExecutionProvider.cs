using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SendFire.Common.Process
{
    public class CommandLineExecutionProvider
    {
               
        public CommandLineExecutionProvider() {
           
        }
        public string ProcessCommands(string[] commands, bool runAsBatch)
        {
            if (commands.Length < 1)
            {
                throw new ArgumentException("No commands specified.");
            }
            var commandLineModels = ParseCommands(commands);
            if (runAsBatch)
            {
                return RunCommandsAsBatch(commandLineModels);
            }
            else
            {
                return RunEachCommandAsProcess(commandLineModels);
            }
        }
        private List<CommandExecutionParamModel> ParseCommands(string[] commands)
        {
            var commandLineModels = new List<CommandExecutionParamModel>();
            for (int i = 0; i < commands.Length; i++)
            {
                var model = new CommandExecutionParamModel();
                if (commands[i].IndexOf(" ") > 0)
                {
                    var index = commands[i].IndexOf(' ');
                    model.Command = commands[i].Substring(0, index);
                    model.Arguments = commands[i].Substring(index);
                }
                else
                {
                    model.Command = commands[i];
                }
                commandLineModels.Add(model);
            }
           
            return commandLineModels;
        }

       

        private string RunEachCommandAsProcess(List<CommandExecutionParamModel> commandLinemodels)
        {
            var processStartInfos = new List<ProcessStartInfo>();
            foreach(var command in commandLinemodels)
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = command.Command,
                    Arguments = command.Arguments,
                    CreateNoWindow = true,

                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,

                    ErrorDialog = false,
                    UseShellExecute = false
                };

                processStartInfos.Add(processStartInfo);
            }
            return RunProcesses(processStartInfos);
  
        }

        private string RunProcesses(List<ProcessStartInfo> processStartInfos)
        {
            var outputBuilder = new StringBuilder();
            try
            {
                foreach (var processInfo in processStartInfos)
                {
                    using (var process = new System.Diagnostics.Process())
                    {
                        //System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo = processInfo;
                        // enable raising events because Process does not raise events by default
                        process.EnableRaisingEvents = true;


                        // attach the event handler for OutputDataReceived before starting the process
                        process.OutputDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
                        process.ErrorDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
                        // start the process
                        // then begin asynchronously reading the output
                        // then wait for the process to exit
                        // then cancel asynchronously reading the output
                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                        process.CancelOutputRead();
                    }
                }

                return outputBuilder.ToString();
            }
            catch (InvalidOperationException iEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return string.Empty;

            
        }
        private string RunCommandsAsBatch(List<CommandExecutionParamModel> commandLineModels)
        {
            var fileName = Path.GetTempFileName() + ".bat";
            var output = string.Empty;
            try
            {
                //To create a batch file
                using (var sw = new StreamWriter(fileName))
                {
                    foreach (var model in commandLineModels)
                    {
                        sw.WriteLine($"{model.Command} {model.Arguments}");
                    }
                }
                //Reset the command with batch file name.
                var newCommand = new CommandExecutionParamModel();
                newCommand.Command = fileName;
                
                output = RunSingleProcess(newCommand);
            }
            catch (Exception ex)
            {
                throw
            }
            finally
            {
                File.Delete(fileName);
            }
            return output;
        }
        private string RunSingleProcess(CommandExecutionParamModel model)
        {
            var process = new System.Diagnostics.Process();
            const int PROCESS_TIMEOUT = 6000;
            var output = string.Empty;
            try
            {
                var outputBuilder = new StringBuilder();

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,

                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    ErrorDialog = false,

                    FileName = model.Command
                };

                process.StartInfo = processStartInfo;
                // enable raising events because Process does not raise events by default
                process.EnableRaisingEvents = true;
                // attach the event handler for OutputDataReceived before starting the process
                process.OutputDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
                process.ErrorDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
                // start the process
                // then begin asynchronously reading the output
                // then wait for the process to exit
                // then cancel asynchronously reading the output
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                process.CancelOutputRead();

                output = outputBuilder.ToString();

                return output;
            }
            catch (Exception e)
            {
               // Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                process.Close();
            }
        }
    }
}
