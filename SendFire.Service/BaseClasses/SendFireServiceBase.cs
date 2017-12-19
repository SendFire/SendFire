using System;
using System.Reflection;
using DasMulli.Win32.ServiceUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendFire.Common.CommandLine;
using SendFire.Common.ExtensionMethods;
using SendFire.Service.Interfaces;

namespace SendFire.Service.BaseClasses
{
    public abstract class SendFireServiceBase : ISendFireService
    {
        public string ApplicationName { get; } = Assembly.GetEntryAssembly().GetName().Name;

        public string ServiceName { get; set; } = Assembly.GetEntryAssembly().GetName().Name;

        public string ApplicationVersion { get; } = Assembly.GetEntryAssembly().GetName().Version.ToString();

        public abstract string GetHelpDescription();

        public abstract string GetServiceDisplayName();

        public abstract string GetServiceDescription();

        public abstract CommandCollection[] GetCommandLineCollections();

        public IServiceProvider ServiceProvider { get; private set; }

        protected IConfiguration Configuration { get; set; }

        protected ILogger Logger { get; set; }
        
        public virtual void Init(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Configuration = serviceProvider.GetService<IConfiguration>();
            Logger = serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Debug, false).CreateLogger(ApplicationName);
        }

        public BaseCommandArgumentSelected ValidateConfiguration()
        {
            if (Configuration[BaseCommandArguments.Help].IsTrue()) return BaseCommandArgumentSelected.DisplayHelp;

            var baseCommandStatus = BaseCommandArgumentSelected.ExecuteProgramAsService;

            if (Configuration[BaseCommandArguments.RegisterService].IsTrue())
                baseCommandStatus = BaseCommandArgumentSelected.RegisterService;
            if (Configuration[BaseCommandArguments.UninstallService].IsTrue())
                baseCommandStatus = BaseCommandArgumentSelected.UninstallService;
            if (Configuration[BaseCommandArguments.ConsoleMode].IsTrue())
                baseCommandStatus = BaseCommandArgumentSelected.ExecuteProgramAsConsole;

            ValidateConfigurationForBaseCommand(baseCommandStatus);
            return(baseCommandStatus);
        }

        /// <summary>
        /// Anyone deriving from this method should throw when configuration is not valid for the given Base command option.
        /// </summary>
        public abstract void ValidateConfigurationForBaseCommand(BaseCommandArgumentSelected baseCommand);

        public abstract void Start();

        public void Start(string[] startupArguments, ServiceStoppedCallback serviceStoppedCallback)
        {
            //File.Create("C:\\GotThere.SendFireServiceBase.txt");
            try
            {
                Start();
            }
            catch (Exception exception)
            {
                Logger.LogCritical($"{ServiceName} Start threw an exception.", exception);
                serviceStoppedCallback.Invoke();
            }
        }

        public abstract void Stop();

        public virtual void UninstallService()
        {
            
        }
    }
}
