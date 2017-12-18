using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendFire.Common.Interfaces;
using SendFire.Service.Interfaces;

namespace SendFire.Service.BaseClasses
{
    public abstract class SendFireServiceBase : ISendFireService
    {
        public string ApplicationName { get; set; } = Assembly.GetEntryAssembly().GetName().Name;

        public string ApplicationVersion { get; } = Assembly.GetEntryAssembly().GetName().Version.ToString();

        public abstract List<string> GetHelpInfo();

        public abstract Dictionary<string, string> GetSwitchMappings();

        public IServiceProvider ServiceProvider { get; private set; }

        protected IConfiguration Configuration { get; set; }

        protected ILogger Logger { get; set; }
        
        public virtual void Init(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Configuration = serviceProvider.GetService<IConfiguration>();
            Logger = serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Debug, false).CreateLogger(ApplicationName);
        }

        public abstract ValidateConfigurationStatus ValidateConfiguration();

        public virtual void InstallAsService()
        {
            throw new NotImplementedException();
        }
        
        public virtual void UninstallAsService()
        {
            throw new NotImplementedException();
        }

        public abstract void StartAsService();

        public abstract void StartAsConsole();

        public abstract void Stop();

        public abstract void DisplayStartup(ValidateConfigurationStatus status);

        public void UninstallService()
        {
            throw new NotImplementedException();
        }
    }
}
