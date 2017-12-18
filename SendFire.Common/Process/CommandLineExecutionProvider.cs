using SendFire.Common.ExtensionMethods;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SendFire.Common.Process
{
    public class CommandLineExecutionProvider
    {

        private CommandExecutionParamModel _commandLineModel;
        //public string ProcessOutput { get; set; }
        public CommandLineExecutionProvider(string[] args) {
            if (args.Length < 1)
            {
                throw new Exception("Can't process..");
            }
            _commandLineModel = ParseArguments(args);
        }

        private CommandExecutionParamModel ParseArguments(string[] args)
        {   
           
            var model = new CommandExecutionParamModel();
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("Command=>" + args[i]);
            }

            if (args.Length > 0 && args[0].ContainsNoCase("-command"))
            {
                var commandArg = args[0].Split('=');
                model.Command = commandArg[1];
            }
            if (args.Length > 1 && args[1].ContainsNoCase("-arguments"))
            {
                var commandArg = args[1].Split('=');
                model.Arguments = commandArg[1];
            }
            if (args.Length > 2 && args[2].ContainsNoCase("-s"))
            {

                model.Arguments = $"%{model.Arguments}%";
                model.IsSysCommand = true;
             
            }
            Console.WriteLine("command=>" + model.Command);
            Console.WriteLine("arguments=>" + model.Arguments);
            return model;
        }

        public string ProcessCommand()
        {
            if (_commandLineModel.IsSysCommand)
            {
                return RunProcessForSystemCommand();
            }
            else
            {
                return RunProcess();
            }
        }

        private string RunProcess()
        {
            var process = new System.Diagnostics.Process();
            const int PROCESS_TIMEOUT = 6000;
            var output = string.Empty;
            try
            {
                var outputBuilder = new StringBuilder();
                ProcessStartInfo processStartInfo;

                processStartInfo = new ProcessStartInfo();
                processStartInfo.CreateNoWindow = true;

                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardError = true;

                processStartInfo.UseShellExecute = false;

                processStartInfo.FileName = _commandLineModel.Command;
                if (!string.IsNullOrEmpty(_commandLineModel.Arguments))
                {
                    processStartInfo.Arguments = _commandLineModel.Arguments;
                }

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


                output = "";
                //var processExited = process.WaitForExit(PROCESS_TIMEOUT);
                //if (processExited == false) // we timed out...
                //{
                //    process.Kill();
                //    throw new Exception("ERROR: Process took too long to finish");
                //}
                //else if (process.ExitCode != 0)
                //{
                //    output = outputBuilder.ToString();
                //    var prefixMessage = "";

                //    throw new Exception($"Process exited with non-zero exit code of: { process.ExitCode}{Environment.NewLine} Output from process: {outputBuilder}");
                //}
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
        private string RunProcessForSystemCommand()
        {
            var fileName = Path.GetTempFileName() + ".bat";
            //To create a batch file
            var sw = new StreamWriter(fileName);
            sw.WriteLine($"{_commandLineModel.Command} {_commandLineModel.Arguments}");
            sw.Close();

            //Reset the fileName with batch file name.
            _commandLineModel.Command = fileName;
            _commandLineModel.Arguments = string.Empty;
            return RunProcess();
        }
       
    }
}
