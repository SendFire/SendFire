using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendFire.Agent.Helpers;
using SendFire.Common.ExtensionMethods;
using SendFire.Common.Interfaces;

namespace SendFire.Agent
{
    internal static class LocalServiceExtensions
    {
        /// <summary>
        /// Add any locally defined services here.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLocalServices(this IServiceCollection services)
        {
            services.AddSingleton(ServiceDescriptor.Singleton<ICommandLineHelper, AgentCommandLineHelper>());
            services.AddSingleton(ServiceDescriptor.Singleton<ISendFireService, AgentService>());
            return(services);
        }
    }

    internal class AgentServicesConfiguration : ISendFireServicesConfiguration
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public ILogger Logger { get; private set;  }

        /// <summary>
        /// Add well known service definitions here.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="args"></param>
        public void ConfigureServices(IServiceCollection services, string[] args)
        {
            ServiceProvider = services
                .AddLogging()
                .AddDefaultConfiguration(args, AgentCommandLineHelper.SwitchMappings)
                .AddLocalServices()
                .BuildServiceProvider();

            Logger = ServiceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Debug).CreateLogger("SendFire.Agent");
            Configuration = ServiceProvider.GetService<IConfiguration>();
        }
    }
}
