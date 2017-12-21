using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendFire.Common.CommandLine;
using SendFire.Common.ExtensionMethods;
using SendFire.Common.Interfaces;
using SendFire.Service.BaseClasses;
using SendFire.Service.Interfaces;

namespace SendFire.Agent
{
    internal class AgentService : SendFireServiceBase
    {
        public string QueueName { get; private set; }
        private BackgroundJobServer _server;
        public override string GetHelpDescription() =>
            "This service acts as the SendFire Command processor, it can be setup to run for either the default queue (which is the fully qualified domain name of this computer) or a named queue defined at service installation. See the optional command line properties below for optional installation and uninstallation command settings.";

        private const string _baseDisplayName = "SendFire Command Processor Agent";
        private string _displayName = _baseDisplayName;
        public override string GetServiceDisplayName() => _displayName;

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
            if (!Configuration["qn"].IsNullOrEmpty()) QueueName = Configuration["qn"].ToLower();
            else QueueName = ServiceProvider.GetService<IEnvironmentManager>().GetMachineName().ToLower().Replace("-", "_");

            if (!Regex.IsMatch(QueueName, @"^[a-z0-9_]+$"))
            {
                throw new ArgumentException($"The queue name must consist of lowercase letters, digits and underscore characters only. Given: '{QueueName}'.");
            }
            ServiceName = $"{ApplicationName} ({QueueName})";
            _displayName = $"{_baseDisplayName} ({QueueName})";
        }

        public override void Start()
        {
            var jobServerOptions = new BackgroundJobServerOptions {Queues = new string[] {QueueName}};
            _server = new BackgroundJobServer(jobServerOptions);   
        }

        public override void Stop()
        {
            _server.Dispose();
        }
    }
}
