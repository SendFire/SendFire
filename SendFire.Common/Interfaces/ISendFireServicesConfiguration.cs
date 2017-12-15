using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SendFire.Common.Interfaces
{
    public interface ISendFireServicesConfiguration
    {
        IServiceProvider ServiceProvider { get; }
        IConfiguration Configuration { get; }
        ILogger Logger { get;  }

        void ConfigureServices(IServiceCollection services, string[] args);
    }
}
