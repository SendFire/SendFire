using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Logging;
using SendFire.Common.CommandLine;
using SendFire.Common.ExtensionMethods;
using SendFire.Service.BaseClasses;
using SendFire.Service.Interfaces;

namespace SendFire.Agent
{
    internal class AgentService : SendFireServiceBase
    {
        public override string GetHelpDescription() =>
            "This service acts as the SendFire Command processor, it can be setup to run for either the default queue (which is the fully qualified domain name of this computer) or a named queue defined at service installation. See the optional command line properties below for optional installation and uninstallation command settings.";

        // TODO - Add Queue name as a part of this too.
        public override string GetServiceDisplayName() => "SendFire Command Processor Agent";

        public override string GetServiceDescription() =>
            "This service acts as the SendFire Command processor, it can be setup to run for either the default queue (which is the fully qualified domain name of this computer) or a named queue defined at service installation.";

        private CommandCollection[] _commandCollections = new []
        {
            new CommandCollection() 
            {
                CollectionName = "runtime-options",
                AvailableArguments = new []
                {
                    new CommandLineArgument() {
                        Command = "qn", CommandValueName = "queue-name", Description = "When specified, this service will register and process messages sent to the [queue-name] queue. If not specified the service will register and process messages from a queue whose name is the FQDN (fully qualified domain name) of the computer. This option also affects the ServiceName as it would be registered and uninstalled in the Service Control Manager, so it should be passed during registration and uninstallation options as well."
                    }
                }
            }
        };

        public override CommandCollection[] GetCommandLineCollections() => _commandCollections;
        
        public override void ValidateConfigurationForBaseCommand(BaseCommandArgumentSelected baseCommand)
        {
            if (!Configuration["qn"].IsNullOrEmpty()) ServiceName = $"{ApplicationName} ({Configuration["qn"]})";
        }

        public override void Start()
        {
            File.Create("C:\\AgenService.Start.txt");
        }

        public override void Stop()
        {
            File.Create("C:\\AgenService.Stop.txt");
        }
    }
}
