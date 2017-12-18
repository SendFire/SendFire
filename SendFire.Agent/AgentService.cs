using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SendFire.Common.ExtensionMethods;
using SendFire.Service.BaseClasses;
using SendFire.Service.Interfaces;

namespace SendFire.Agent
{
    internal class AgentService : SendFireServiceBase
    {
        private List<string> _helpInfo;
        public override List<string> GetHelpInfo()
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
                    $"{ApplicationName} ({ApplicationVersion}) ",
                    $"Usage: {ApplicationName} [runtime-options]",
                    $"Usage: {ApplicationName} [service-install-options]",
                    $"Usage: {ApplicationName} [service-uninstall-options]",
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

        private readonly Dictionary<string, string> _switchMappings = new Dictionary<string, string>()
        {
            { "-c", "console" },
            { "-?", "help" }
        };

        public override Dictionary<string, string> GetSwitchMappings() => _switchMappings;

        public override ValidateConfigurationStatus ValidateConfiguration()
        {
            if (Configuration["help"].IsTrue()) return ValidateConfigurationStatus.DisplayHelp;

            // TODO: Need to validate everything needed was passed on command line given main arguments and throw if invalid.

            if (Configuration["console"].IsTrue()) return ValidateConfigurationStatus.ExecuteProgramAsConsole;

            ApplicationName = $"{ApplicationName} (Windows Service Mode)";
            return ValidateConfigurationStatus.ExecuteProgramAsService;
        }

        public override void DisplayStartup(ValidateConfigurationStatus status)
        {
            // Display help if asked.
            if (status == ValidateConfigurationStatus.DisplayHelp)
            {
                foreach (var info in GetHelpInfo())
                {
                    Logger.LogInformation(info);
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

        public override void StartAsService()
        {
            throw new NotImplementedException();
        }

        public override void StartAsConsole()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
