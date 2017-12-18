using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendFire.Common.Configuration;
using SendFire.Common.Interfaces;

namespace SendFire.Common.ExtensionMethods
{
    public static class ConfigurationExtensions
    {

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from the command line using the specified switch mappings.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="args">The command line args.</param>
        /// <param name="switchMappings">The switch mappings.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddProperCommandLine(
            this IConfigurationBuilder configurationBuilder,
            string[] args,
            IDictionary<string, string> switchMappings = null)
        {
            configurationBuilder.Add(new ProperCommandLineConfigurationSource { Args = args, SwitchMappings = switchMappings });
            return configurationBuilder;
        }

        /// <summary>
        /// Add a default application configuration into a passsed configuration builder. Allows for starting with a 
        /// default configuration setup but adding special instructions later (like command lines).
        /// </summary>
        /// <param name="configBuilder"></param>
        /// <param name="executionEnvironment"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddDefaultConfiguration(this IConfigurationBuilder configBuilder, IExecutionEnvironment executionEnvironment)
        {
            configBuilder.SetBasePath(executionEnvironment.ServiceRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            if (executionEnvironment.IsDevelopment())
            {
                var devAppSettingsFile = Path.Combine(AppContext.BaseDirectory, $"appsettings.{executionEnvironment.EnvironmentName}.json");
                if (File.Exists(devAppSettingsFile)) configBuilder.AddJsonFile(devAppSettingsFile);
                else
                {
                    devAppSettingsFile = Path.Combine(AppContext.BaseDirectory,
                        string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{executionEnvironment.EnvironmentName}.json");
                    configBuilder.AddJsonFile(devAppSettingsFile, true);
                }
            }
            else
            {
                configBuilder
                    .AddJsonFile($"appsettings.{executionEnvironment.EnvironmentName}.json", true);
            }

            return configBuilder;
        }

        /// <summary>
        /// Add a default application configuration into dependency injection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="executionEnvironment"></param>
        public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services, IExecutionEnvironment executionEnvironment)
        {
            services.Add(new ServiceDescriptor(typeof(IConfiguration),
                provider => new ConfigurationBuilder().AddDefaultConfiguration(executionEnvironment).Build(),
                ServiceLifetime.Singleton));
            return services;
        }

        /// <summary>
        /// Add a default application configuration into dependency injection including the command line.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services, 
            IExecutionEnvironment executionEnvironment,
            string[] args,
            IDictionary<string, string> switchMappings = null)
        {
            services.Add(new ServiceDescriptor(typeof(IConfiguration),
                provider => new ConfigurationBuilder()
                    .AddDefaultConfiguration(executionEnvironment)
                    .AddProperCommandLine(args, switchMappings)
                    .Build(),
                ServiceLifetime.Singleton));
            return services;
        }
    }
}
