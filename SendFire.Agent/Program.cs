using System;
using Microsoft.Extensions.DependencyInjection;
using SendFire.Common.Environment;

namespace SendFire.Agent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var agentServiceFactory = new AgentServiceFactory();
                var environmentManager = new EnvironmentManager();
                // Base Application Services Configuration, wires up all DI of app.
                agentServiceFactory.ConfigureServices(environmentManager, new ExecutionEnvironment(environmentManager), new ServiceCollection(), args);

                agentServiceFactory.ValidateAndExecuteService();
            }
            catch (Exception ex)
            {
                // Catastrophic failure of some type as base should not pass exception out of itself.
                Console.WriteLine("Application Framework Exception!");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
