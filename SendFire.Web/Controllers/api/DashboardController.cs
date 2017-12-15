using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace SendFire.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Counts()
        {
            Hangfire.Storage.IMonitoringApi monitor = JobStorage.Current.GetMonitoringApi();
            var succeeded = monitor.SucceededListCount();
            var failed = monitor.FailedCount();
            var processing = monitor.ProcessingCount();
            var jobs = monitor.ScheduledJobs(0, 100).Count;
            var queues = monitor.Queues().ToList();
            var queued = new Dictionary<string, long>();
            long enqueued = 0;
            foreach(var q in queues) {
                var count = monitor.EnqueuedCount(q.Name);
                queued.Add(q.Name, count);
                enqueued += count;
            }

            var list = new List<DashboardCount>();
            list.Add(new DashboardCount() {
                Label ="Succeeded",
                Count = succeeded
            });

            list.Add(new DashboardCount() {
                Label ="Failed",
                Count = failed
            });

            list.Add(new DashboardCount() {
                Label ="Processing",
                Count = processing
            });

            list.Add(new DashboardCount() {
                Label ="Jobs",
                Count = jobs
            });

            list.Add(new DashboardCount() {
                Label ="Enqueued",
                Count = enqueued
            });


            return Json(list);
        }

    }

    public class DashboardCount {
        public string Label {get;set;}
        public long Count {get;set;}
    }
}