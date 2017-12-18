using System;
using SendFire.Common.Interfaces;

namespace SendFire.Common.Environment
{
    public class EnvironmentManager : IEnvironmentManager
    {
        public string GetEnvironmentVariable(string variable)
        {
            return System.Environment.GetEnvironmentVariable(variable);
        }

        public void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            System.Environment.SetEnvironmentVariable(variable, value, target);
        }
    }
}
