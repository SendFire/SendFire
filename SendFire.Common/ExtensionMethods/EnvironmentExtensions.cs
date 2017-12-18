using System;
using System.Collections.Generic;
using System.Text;
using SendFire.Common.Environment;
using SendFire.Common.Interfaces;

namespace SendFire.Common.ExtensionMethods
{
    public static class EnvironmentExtensions
    {
        public static bool IsDevelopment(this IExecutionEnvironment executionEnvironment)
        {
            return(executionEnvironment.EnvironmentName.CompareNoCase(ExecutionEnvironment.Development));
        }

        public static bool IsStaging(this IExecutionEnvironment executionEnvironment)
        {
            return (executionEnvironment.EnvironmentName.CompareNoCase(ExecutionEnvironment.Staging));
        }
        public static bool IsProduction(this IExecutionEnvironment executionEnvironment)
        {
            return (executionEnvironment.EnvironmentName.CompareNoCase(ExecutionEnvironment.Production));
        }

        public static bool IsEnvironemnt(this IExecutionEnvironment executionEnvironment, string environmentName)
        {
            return (executionEnvironment.EnvironmentName.CompareNoCase(environmentName));
        }
    }
}
