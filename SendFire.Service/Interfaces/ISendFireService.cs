using System;
using System.Collections.Generic;
using SendFire.Common.Interfaces;

namespace SendFire.Service.Interfaces
{
    public enum ValidateConfigurationStatus
    {
        DisplayHelp,
        ExecuteProgramAsService,
        ExecuteProgramAsConsole,
        InstallService,
        UninstallService
    }

    public interface ISendFireService
    {
        #region Properties Provided by Service Instance
        
        /// <summary>
        /// The name of the application. This property is automatically set by the host to the assembly containing the application entry point but can be modified
        /// by the service specific code to support multiple instances of the same service on the same box that perform different functions.
        /// </summary>
        string ApplicationName { get; set; }

        string ApplicationVersion { get; }

        List<string> GetHelpInfo();

        Dictionary<string, string> GetSwitchMappings();

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
        ValidateConfigurationStatus ValidateConfiguration();

        /// <summary>
        /// Logs startup information based upon passed configuration status.
        /// </summary>
        /// <param name="status"></param>
        void DisplayStartup(ValidateConfigurationStatus status);

        void InstallAsService();
        void UninstallService();

        void StartAsService();
        void StartAsConsole();

        void Stop();
    }
}
