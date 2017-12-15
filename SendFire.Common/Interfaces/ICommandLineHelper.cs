using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendFire.Common.ExtensionMethods;

namespace SendFire.Common.Interfaces
{
    public enum ParseConfigurationStatus
    {
        DisplayHelp,
        ExecuteProgramAsService,
        ExecuteProgramAsConsole,
        InstallService,
        UninstallService
    }

    public interface ICommandLineHelper
    {
        IConfiguration Configuration { get; }
        
        string AppBinaryName { get; }

        string AppTitle { get; }

        string AppVersion { get; }

        List<string> HelpInfo { get; }

        /// <summary>
        /// This method should throw when configuration is missing or invalid so that execution will NOT occur. Exception should contain
        /// information you wish to display to the user about what is invalid. Help will also be displayed.
        /// </summary>
        /// <returns></returns>
        ParseConfigurationStatus ValidateConfiguration();

        /// <summary>
        /// Console logs startup information based upon passed configuration status.
        /// </summary>
        /// <param name="status"></param>
        void DisplayStartup(ParseConfigurationStatus status);
    }
}
