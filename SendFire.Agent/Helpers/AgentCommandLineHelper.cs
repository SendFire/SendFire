using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendFire.Common.ExtensionMethods;
using SendFire.Common.Interfaces;

namespace SendFire.Agent.Helpers
{
    internal class AgentCommandLineHelper : ICommandLineHelper
    {
        public IConfiguration Configuration { get; }
        public AgentCommandLineHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Properties

        public string AppBinaryName { get; } = Assembly.GetEntryAssembly().GetName().Name;

        public string AppTitle { get; private set; } = Assembly.GetEntryAssembly().GetName().Name;

        public string AppVersion { get; } = Assembly.GetEntryAssembly().GetName().Version.ToString();

        public static Dictionary<string, string> SwitchMappings { get; } = new Dictionary<string, string>()
        {
            { "-c", "console" },
            { "-?", "help" }
        };

        private List<string> _helpInfo;
        public List<string> HelpInfo
        {
            get
            {
                if (_helpInfo == null)
                {
                    _helpInfo = new List<string>
                    {
                        @"This service acts as the SendFire Command processor, it can be setup to run for either",
                        @"the default queue (which is the fully qualified domain name of this computer) or a named",
                        @"queue defined at service installation. See the optional command line properties below for",
                        @"optional installation and uninstallation command settings.",
                        @" ",
                        $"{AppBinaryName} ({AppVersion}) ",
                        $"Usage: {AppBinaryName} [runtime-options]",
                        $"Usage: {AppBinaryName} [service-install-options]",
                        $"Usage: {AppBinaryName} [service-uninstall-options]",
                        @" ",
                        @"runtime-options:",
                        @"  -c|--console        Run this app in console mode, if this is NOT SET then the application",
                        @"                      will attempt to start as a service assuming it is being started as that.",
                        @"  -?|--help           Display this help information.",
                        @"  --qn:[queue-name]   When specified, this service will register and process messages sent to",
                        @"                      the [queue-name] queue. If not specified the service will register and process",
                        @"                      messages from a queue whose name if the FQDN (fully qualified domain name) of the",
                        @"                      computer."
                    };
                }

                return (_helpInfo);
            }
        }

        public List<string> ErrorInfo { get; } = new List<string>();

        #endregion

        #region Helper Methods

        public ParseConfigurationStatus ValidateConfiguration()
        {
            if (Configuration["help"].IsTrue()) return ParseConfigurationStatus.DisplayHelp;

            // TODO: Need to validate everything needed was passed on command line given main arguments and throw if invalid.

            if (Configuration["console"].IsTrue()) return ParseConfigurationStatus.ExecuteProgramAsConsole;

            AppTitle = $"{AppBinaryName} (Windows Service Mode)";
            return ParseConfigurationStatus.ExecuteProgramAsService;
        }

        public void DisplayStartup(ParseConfigurationStatus status)
        {
            // Display help if asked.
            if (status == ParseConfigurationStatus.DisplayHelp)
            {
                foreach (var info in HelpInfo)
                {
                    Console.WriteLine(info);
                }
            }
            // Otherwise display the parameters that will be used for this execution run.
            else
            {
                //if (model.ParameterInfo.Count > 0)
                //{
                //    Logger.PushHeaderInfo($"{CommandLineModel.AppBinaryName} Execution Parameters");
                //    foreach (var info in model.ParameterInfo)
                //    {
                //        Logger.PushInfo(info);
                //    }
                //}
                //if (Configuration["console"].IsTrue())
                //{
                //    Logger.LogInfo($"{AppTitle} Started on {DateTime.Now} in Console Mode.");
                //}
                //else
                //{
                //    Logger.LogInfo($"{CommandLineModel.AppBinaryName} Started on {DateTime.Now} in Service Mode.");
                //}
            }
        }

        #endregion

    }
}
