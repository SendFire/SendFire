using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendFire.Common;
using SendFire.Common.Interfaces;

namespace SendFire.Service.Interfaces
{
    public interface ISendFireServiceFactory
    {
        IServiceProvider ServiceProvider { get; }
        ILogger Logger { get;  }

        ISendFireService GetServiceSingleton();

        void ConfigureServices(IEnvironmentManager environmentManager, IExecutionEnvironment executionEnvironment, IServiceCollection services, string[] args);

        void ValidateAndExecuteService();
    }
}
