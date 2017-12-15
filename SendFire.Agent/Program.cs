using System;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendFire.Agent.Helpers;
using SendFire.Common.ExtensionMethods;
using SendFire.Common.Interfaces;

namespace SendFire.Agent
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesConfiguration = new AgentServicesConfiguration();
            var commandLineHelper = (ICommandLineHelper) null;
            var logger = (ILogger)null;

            try
            {
                // Base Application Services Configuration, wires up all DI of app.
                servicesConfiguration.ConfigureServices(new ServiceCollection(), args);

                commandLineHelper = servicesConfiguration.ServiceProvider.GetService<ICommandLineHelper>();
                logger = servicesConfiguration.Logger;

                var status = commandLineHelper.ValidateConfiguration();

                Console.Title = commandLineHelper.AppTitle;
                commandLineHelper.DisplayStartup(status);

                switch(status)
                {
                    case ParseConfigurationStatus.DisplayHelp:
                        Environment.ExitCode = 1;
                        Thread.Sleep(500); // Allow NLog Console Logging.
                        break;

                    case ParseConfigurationStatus.ExecuteProgramAsConsole:
                        //var reliabilityService = new freebyRunner(model);
                        //reliabilityService.OnStartConsoleMode();

                        //Console.WriteLine("Hit any key to stop");
                        //Console.ReadKey();
                        //Console.WriteLine("Key Registered, Stopping ....");

                        //reliabilityService.OnStopConsoleMode();
                        //Thread.Sleep(500); // Allow NLog Console Logging.
                        break;

                    case ParseConfigurationStatus.ExecuteProgramAsService:
                        //var service = new ServiceBase[]
                        //{
                        //    new freebyRunner(model),
                        //};
                        //ServiceBase.Run(service);
                        break;

                    case ParseConfigurationStatus.InstallService:
                        // TODO: Service Install.
                        break;

                    case ParseConfigurationStatus.UninstallService:
                        // TODO: Service Uninstall.
                        break;
                }
            }
            catch (Exception ex)
            {
                // Catastrophic failure of DI and configuration building, assume invalid configuration and display help.
                if (commandLineHelper == null)
                {
                    commandLineHelper = new AgentCommandLineHelper(null);
                }
                var errorTitle = $"{commandLineHelper.AppBinaryName} Startup Exception!";

                if (logger != null)
                {
                    logger.LogCritical(errorTitle, ex);
                }
                else
                {
                    Console.WriteLine(errorTitle);
                    Console.WriteLine(ex.Message);
                }
                commandLineHelper.DisplayStartup(ParseConfigurationStatus.DisplayHelp);
                Thread.Sleep(500); // Allow Console / Other Logging if its on a background thread.
            }
        }
    }
}
