using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
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
            //var sendFireDb =@"Server=.\\sqlexpress; Database=SendFire; User Id=SendFire; Password=SendFire2017#!";
            var connectionString = Configuration.GetConnectionString("SendFireDB");
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);
            //services.AddHangfire(x => x.UseSqlServerStorage("SendFireDB"));
            //GlobalConfiguration.Configuration.UseSqlServerStorage(sendFireDB);
            //JobStorage.Current = new SqlServerStorage(sendFireDB);

        }
    }
}
