using SendFire.Common.ExtensionMethods;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SendFire.Common.Process
{
    public class CommandLineExecutionProvider
    {

        public List<CommandExecutionParamModel> _commandLineModels;
        private bool _runAsBatch;
        //public string ProcessOutput { get; set; }
        public CommandLineExecutionProvider(string[] args, bool runAsBatch) {
            if (args.Length < 1)
            {
                throw new ArgumentException("No command line specified.");
            }
            _runAsBatch = runAsBatch;
            _commandLineModels = ParseArguments(args);
        }

        private List<CommandExecutionParamModel> ParseArguments(string[] args)
        {
            _commandLineModels = new List<CommandExecutionParamModel>();
            for (int i = 0; i < args.Length; i++)
            {
                var model = new CommandExecutionParamModel();
                if (args[i].IndexOf(" ") > 0)
                {
                    var index = args[i].IndexOf(' ');
                   // var arguments = args[i].Split(' ');
                    model.Command = args[i].Substring(0, index);
                    model.Arguments = args[i].Substring(index);
                }
                else
                {
                    model.Command = args[i];
                }
                _commandLineModels.Add(model);
            }

           
            return _commandLineModels;
        }

        public string ProcessCommand()
        {
            if (_runAsBatch)
            {
                return RunProcessForSystemCommand();
            }
            else
            {
                //   return RunProcess();
                return RunMultipleProcess();
            }
        }

        private string RunMultipleProcess()
        {
            var processStartInfos = new List<ProcessStartInfo>();
            foreach(var command in _commandLineModels)
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

            }
            catch (Exception ex)
            {


            }
            return string.Empty;

            
        }
        //private string RunProcess()
        //{
        //    var process = new System.Diagnostics.Process();
        //    const int PROCESS_TIMEOUT = 6000;
        //    var output = string.Empty;
        //    try
        //    {
        //        var outputBuilder = new StringBuilder();
        //        ProcessStartInfo processStartInfo;

        //        processStartInfo = new ProcessStartInfo();
        //        processStartInfo.CreateNoWindow = true;

        //        processStartInfo.RedirectStandardOutput = true;
        //        processStartInfo.RedirectStandardInput = true;
        //        processStartInfo.RedirectStandardError = true;

        //        processStartInfo.UseShellExecute = false;


        //        //processStartInfo.FileName = _commandLineModels.Command;
        //        //if (!string.IsNullOrEmpty(_commandLineModels.Arguments))
        //        //{
        //        //    processStartInfo.Arguments = _commandLineModels.Arguments;
        //        //}

        //        process.StartInfo = processStartInfo;
        //        // enable raising events because Process does not raise events by default
        //        process.EnableRaisingEvents = true;
        //        // attach the event handler for OutputDataReceived before starting the process
        //        process.OutputDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
        //        process.ErrorDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
        //        // start the process
        //        // then begin asynchronously reading the output
        //        // then wait for the process to exit
        //        // then cancel asynchronously reading the output
        //        process.Start();
        //        process.BeginOutputReadLine();
        //        process.BeginErrorReadLine();
        //        process.WaitForExit();
        //        process.CancelOutputRead();


        //        output = "";
        //        //var processExited = process.WaitForExit(PROCESS_TIMEOUT);
        //        //if (processExited == false) // we timed out...
        //        //{
        //        //    process.Kill();
        //        //    throw new Exception("ERROR: Process took too long to finish");
        //        //}
        //        //else if (process.ExitCode != 0)
        //        //{
        //        //    output = outputBuilder.ToString();
        //        //    var prefixMessage = "";

        //        //    throw new Exception($"Process exited with non-zero exit code of: { process.ExitCode}{Environment.NewLine} Output from process: {outputBuilder}");
        //        //}
        //        output = outputBuilder.ToString();

        //        return output;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    finally
        //    {
        //        process.Close();
        //    }
        //    return output;
        //}
        private string RunProcessForSystemCommand()
        {
            var fileName = Path.GetTempFileName() + ".bat";
            //To create a batch file
            using(var sw = new StreamWriter(fileName))
            {
                foreach(var model in _commandLineModels)
                {
                    sw.WriteLine($"{model.Command} {model.Arguments}");
                }
            }

            var newCommand = new CommandExecutionParamModel();
            newCommand.Command = fileName;

            //Reset the fileName with batch file name.
            return RunSingleProcess(newCommand);
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
                Console.WriteLine(e.Message);
            }
            finally
            {
                process.Close();
            }
            return output;
        }
    }
}
