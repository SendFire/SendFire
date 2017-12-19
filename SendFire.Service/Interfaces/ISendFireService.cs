using System;
using System.Collections.Generic;
using DasMulli.Win32.ServiceUtils;
using SendFire.Common.CommandLine;
using SendFire.Common.Interfaces;
using SendFire.Service.BaseClasses;

namespace SendFire.Service.Interfaces
{
    public interface ISendFireService : IWin32Service
    {
        #region Properties Provided by Service Instance
        
        /// <summary>
        /// The name of the application. 
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// The name of the service when installed into the Service Control Manager. This property is automatically set by the host to the assembly containing the application entry point but can be modified
        /// by the service specific code to support multiple instances of the same service on the same box that perform different functions.
        /// </summary>
        //string ServiceName { get; set; } <-- Defined by IWin32Service, same usage.

        string ApplicationVersion { get; }

        string GetHelpDescription();

        string GetServiceDisplayName();

        string GetServiceDescription();

        CommandCollection[] GetCommandLineCollections();

        #endregion

        #region ServiceProvider provided to Service by Factory, all other services can be derived from this provider.

        IServiceProvider ServiceProvider { get; }
        
        #endregion

        /// <summary>
        /// Allows for initialization of base service services before validating startup configuration.
        /// </summary>
        /// <param name="serviceProvider"></param>
        void Init(IServiceProvider serviceProvider);

        /// <summary>
        /// This method should throw when configuration is missing or invalid so that execution will NOT occur. Exception should contain
        /// information you wish to display to the user about what is invalid. Help will also be displayed.
        /// </summary>
        /// <returns></returns>
        BaseCommandArgumentSelected ValidateConfiguration();
        
        void Start();

        //void Stop(); <-- Defined by IWin32Service, same usage.

        void UninstallService();
    }
}
