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
        public IActionResult Enqueue([FromBody]CommandRequest commandRequest) {
            BackgroundJob.Enqueue(() => JobJob.DoJob(commandRequest.Command));
            return Json("Ok");
        }

    }
    public static class JobJob {
        public static string DoJob(string say) {
            var test = "you don't " + say;
            return test;
        }
    }

    public class CommandRequest {
        public string Command {get;set;}
    }
}
