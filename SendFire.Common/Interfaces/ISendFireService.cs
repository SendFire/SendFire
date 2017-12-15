using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SendFire.Common.Interfaces
{
    public interface ISendFireService
    {
        void OnStart();

        void OnStop();
    }
}
