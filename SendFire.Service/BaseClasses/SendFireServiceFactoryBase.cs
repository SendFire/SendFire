using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendFire.Common.ExtensionMethods;
using SendFire.Common.Interfaces;
using SendFire.Service.Interfaces;

namespace SendFire.Service.BaseClasses
{
    public abstract class SendFireServiceFactoryBase : ISendFireServiceFactory
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public ILogger Logger { get; private set; }

        public abstract ISendFireService GetServiceSingleton();

        public void ConfigureServices(IEnvironmentManager environmentManager, IExecutionEnvironment executionEnvironment, IServiceCollection services, string[] args)
        {
            var basicServicesProvider = services
                .AddLogging()
                .AddSingleton(environmentManager)
                .AddSingleton(executionEnvironment)
                .AddDefaultConfiguration(executionEnvironment, args, GetServiceSingleton().GetSwitchMappings())
                .BuildServiceProvider();

            Logger = basicServicesProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Debug, false).CreateLogger("ServiceStartup");

            OnConfigureServices(services, args);
            basicServicesProvider.GetService<IConfiguration>();
            ServiceProvider = services.BuildServiceProvider();
        }

        protected abstract void OnConfigureServices(IServiceCollection services, string[] args);

        public void ValidateAndExecuteService()
        {
            var iSendFireService = GetServiceSingleton();

            try
            {
                iSendFireService.Init(ServiceProvider);
                var status = iSendFireService.ValidateConfiguration();

                Console.Title = iSendFireService.ApplicationName;
                iSendFireService.DisplayStartup(status);

                switch (status)
                {
                    case ValidateConfigurationStatus.DisplayHelp:
                        Environment.ExitCode = 1;
                        Thread.Sleep(500); // Allow NLog Console Logging.
                        break;

                    case ValidateConfigurationStatus.ExecuteProgramAsConsole:
                        //var reliabilityService = new freebyRunner(model);
                        //reliabilityService.OnStartConsoleMode();

                        //Console.WriteLine("Hit any key to stop");
                        //Console.ReadKey();
                        //Console.WriteLine("Key Registered, Stopping ....");

                        //reliabilityService.OnStopConsoleMode();
                        //Thread.Sleep(500); // Allow NLog Console Logging.
                        break;

                    case ValidateConfigurationStatus.ExecuteProgramAsService:
                        //var service = new ServiceBase[]
                        //{
                        //    new freebyRunner(model),
                        //};
                        //ServiceBase.Run(service);
                        break;

                    case ValidateConfigurationStatus.InstallService:
                        // TODO: Service Install.
                        break;

                    case ValidateConfigurationStatus.UninstallService:
                        // TODO: Service Uninstall.
                        break;
                }
            }
            catch (Exception ex)
            {
                var errorTitle = $"{iSendFireService.ApplicationName} Startup Exception!";

                // Catastrophic failure of DI and configuration building, display exception and shut down.
                if (Logger != null)
                {
                    Logger.LogCritical(errorTitle, ex);
                    iSendFireService.DisplayStartup(ValidateConfigurationStatus.DisplayHelp);
                    Thread.Sleep(500); // Allow Console / Other Logging if its on a background thread.
                }
                else
                {
                    Console.WriteLine(errorTitle);
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
