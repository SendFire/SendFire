using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace SendFire.Web.Controllers
{
    public class JobsController : Controller
    {    
        public IActionResult Succeeded()
        {
            Hangfire.Storage.IMonitoringApi monitor = JobStorage.Current.GetMonitoringApi();
            var jobs = monitor.SucceededJobs(0, 10);
            return Json(jobs.Select(j => j.Key).ToList());
        }

        public IActionResult Failed()
        {
            Hangfire.Storage.IMonitoringApi monitor = JobStorage.Current.GetMonitoringApi();
            var jobs = monitor.SucceededJobs(0, 10);
            return Json(jobs.Select(j => j.Key).ToList());
        }

        [HttpPost]
        public IActionResult Enqueue([FromBody]CommandRequest commandRequest)
        {
            var jobs = new[] {commandRequest.Command};
            var id = BackgroundJob.EnqueueTo("default", () => new SendFire.Common.Process.CommandLineExecutionProvider().ProcessCommands(jobs, true, 0));
            return Json(new {
                id = id
            });
        }

    }

    public class CommandRequest {
        public string Command {get;set;}
    }
}
