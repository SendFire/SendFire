using System;
using System.Runtime.InteropServices;
using SendFire.Common.Interfaces;

namespace SendFire.Common.Environment
{
    public class EnvironmentManager : IEnvironmentManager
    {
        public string GetEnvironmentVariable(string variable)
        {
            return System.Environment.GetEnvironmentVariable(variable);
        }

        public OSPlatform GetOSPlatform()
        {
            //Set default as window
            OSPlatform osPlatform = OSPlatform.Windows;
            // Check if it's windows 
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            osPlatform = isWindows ? OSPlatform.Windows : osPlatform;
            // Check if it's osx 
            bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            osPlatform = isOSX ? OSPlatform.OSX : osPlatform;
            // Check if it's Linux 
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            osPlatform = isLinux ? OSPlatform.Linux : osPlatform;
            return osPlatform;
        }

        public string GetMachineName()
        {
            return System.Environment.MachineName;
        }

        public void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            System.Environment.SetEnvironmentVariable(variable, value, target);
        }
    }
}
