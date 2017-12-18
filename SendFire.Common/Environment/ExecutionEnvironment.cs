using System;
using SendFire.Common.Interfaces;
using SendFire.Common.ExtensionMethods;

namespace SendFire.Common.Environment
{
    public class ExecutionEnvironment : IExecutionEnvironment
    {
        public const string Production = "Production";
        public const string Staging = "Staging";
        public const string Development = "Development";

        public ExecutionEnvironment(IEnvironmentManager environment)
        {
            var env = environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env.IsNullOrEmpty()) env = Production;

            EnvironmentName = env;

            // TODO: Directory.GetCurrentDirectory() instead?
            ServiceRootPath = AppContext.BaseDirectory;
        }

        public ExecutionEnvironment(string environmentName, string serviceRootPath)
        {
            EnvironmentName = environmentName;
            ServiceRootPath = serviceRootPath;
        }

        public string EnvironmentName { get; }
        public string ServiceRootPath { get; }
    }
}
