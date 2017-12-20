using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SendFire.Common.Interfaces;
using System.Runtime.InteropServices;
using SendFire.Common.Environment;

namespace SendFire.Common.Process
{
    public class CommandLineExecutionProvider
    {
               
        public CommandLineExecutionProvider() {
           
        }
        public string ProcessCommands(string[] commands, bool runAsBatch, int processTimeoutMs=0)
        {
            if (commands.Length < 1)
            {
                throw new ArgumentException("No commands specified.");
            }
            var commandLineModels = ParseCommands(commands);
            if (runAsBatch)
            {
                return RunCommandsAsBatch(commandLineModels, processTimeoutMs);
            }
            else
            {
                return RunEachCommandAsProcess(commandLineModels, processTimeoutMs);
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

       

        private string RunEachCommandAsProcess(List<CommandExecutionParamModel> commandLinemodels, int processTimeoutMs)
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
            return RunProcesses(processStartInfos, processTimeoutMs);
  
        }

        private string RunProcesses(List<ProcessStartInfo> processStartInfos, int processTimeoutMs)
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
                        if (processTimeoutMs > 0)
                        {
                            var processExited = process.WaitForExit(processTimeoutMs);
                            if (!processExited)
                            {
                                process.Kill();
                                throw new TimeoutException($"Process did not finish in {processTimeoutMs} ms.");
                            }
                        }
                        else
                        {
                            process.WaitForExit();
                        }
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
            
        }
        private string RunCommandsAsBatch(List<CommandExecutionParamModel> commandLineModels, int processTimeoutMs)
        {
            var fileModel = GetExecuteBatchFileModel();
            var output = string.Empty;
            try
            {
                //To create a batch file
                using (var sw = new StreamWriter(fileModel.FileName))
                {
                    if (fileModel.IsUnixOS)
                    {
                        sw.WriteLine("#!/bin/bash");
                    }
                    foreach (var model in commandLineModels)
                    {
                        sw.WriteLine($"{model.Command} {model.Arguments}");
                    }
                }
                //Reset the command with batch file name.
                var newCommand = new CommandExecutionParamModel();
                newCommand.Command = fileModel.FileName;
                
                output = RunSingleProcess(newCommand, processTimeoutMs);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                File.Delete(fileModel.FileName);
            }
            return output;
        }
        private CommandExecutionFileModel GetExecuteBatchFileModel()
        {
            var model = new CommandExecutionFileModel();
            var environment = new EnvironmentManager();
            var osPlatForm = environment.GetOSPlatform();

            var extension = "";
            if(osPlatForm == OSPlatform.Windows)
            {
                extension = ".bat";
                model.IsUnixOS = false;
            }

            if(osPlatForm == OSPlatform.Linux || osPlatForm == OSPlatform.OSX)
            {
                extension = ".sh";
                model.IsUnixOS = true;
            }
            model.FileName = $"{Path.GetTempFileName()}{extension}";
            return model;
            
        }
        private string RunSingleProcess(CommandExecutionParamModel model, int processTimeoutMs)
        {
            var process = new System.Diagnostics.Process();
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
                if(processTimeoutMs > 0)
                {
                   var processExited= process.WaitForExit(processTimeoutMs);
                    if (!processExited)
                    {
                        process.Kill();
                        throw new TimeoutException($"Process did not finish in {processTimeoutMs} ms.");
                    }
                }
                else
                {
                    process.WaitForExit();
                }
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
