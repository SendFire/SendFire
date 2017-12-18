using Microsoft.Extensions.DependencyInjection;
using SendFire.Service.BaseClasses;
using SendFire.Service.Interfaces;

namespace SendFire.Agent
{
    internal class AgentServiceFactory : SendFireServiceFactoryBase
    {
        private readonly ISendFireService _agentService = new AgentService();
        public override ISendFireService GetServiceSingleton() => _agentService;

        protected override void OnConfigureServices(IServiceCollection services, string[] args)
        {
            //services.AddSingleton<ISendFireService, AgentService>();
        }
    }
}
