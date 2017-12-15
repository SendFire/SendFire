using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendFire.Common.Configuration;

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
        /// <returns></returns>
        public static IConfigurationBuilder AddDefaultConfiguration(this IConfigurationBuilder configBuilder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment.IsNullOrEmpty()) environment = "Production";

            // TODO: Directory.GetCurrentDirectory() instead?
            configBuilder.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            if (environment.CompareNoCase("Development"))
            {
                var devAppSettingsFile = Path.Combine(AppContext.BaseDirectory, $"appsettings.{environment}.json");
                if (File.Exists(devAppSettingsFile)) configBuilder.AddJsonFile(devAppSettingsFile);
                else
                {
                    devAppSettingsFile = Path.Combine(AppContext.BaseDirectory,
                        string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json");
                    configBuilder.AddJsonFile(devAppSettingsFile, true);
                }
            }
            else
            {
                configBuilder
                    .AddJsonFile($"appsettings.{environment}.json", true);
            }

            return configBuilder;
        }

        /// <summary>
        /// Add a default application configuration into dependency injection.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IConfiguration),
                provider => new ConfigurationBuilder().AddDefaultConfiguration().Build(),
                ServiceLifetime.Singleton));
            return services;
        }

        /// <summary>
        /// Add a default application configuration into dependency injection including the command line.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services,
            string[] args,
            IDictionary<string, string> switchMappings = null)
        {
            services.Add(new ServiceDescriptor(typeof(IConfiguration),
                provider => new ConfigurationBuilder().AddDefaultConfiguration().AddProperCommandLine(args, switchMappings).Build(),
                ServiceLifetime.Singleton));
            return services;
        }
    }
}
