using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage.Monitoring;
using Microsoft.AspNetCore.Mvc;
using SendFire.Common.Data.Implementations;
using SendFire.Common.Data.Models;

namespace SendFire.Web.Controllers
{
    public class QueuesController : Controller
    {    
        private ServerQueueData _serverQueueData;
        public QueuesController(ServerQueueData serverQueueData)
        {
            _serverQueueData = serverQueueData;
        }
        public IActionResult List()
        {
            Hangfire.Storage.IMonitoringApi monitor = JobStorage.Current.GetMonitoringApi();
            var queues = monitor.Queues().ToList();

            var serverqueues = _serverQueueData.FindAll();

            return Json(serverqueues.Select(q => new QueueModel(q)));
            //return Json(queues.Select(q => new QueueModel(q)));
        }

    }

    public class QueueModel {
        public QueueModel()
        {
        }
        public QueueModel(ServerQueue model) {
            Name = model.Description;
            JobCount = 0;
            NextJobs = new List<JobModel>();
        }

        public QueueModel(QueueWithTopEnqueuedJobsDto model) {
            Name = model.Name;
            JobCount = model.Length;
            NextJobs = model.FirstJobs.Select(j => new JobModel(j.Key, j.Value));
        }
        public string Name {get;set;}
        public long JobCount {get;set;}
        public IEnumerable<JobModel> NextJobs {get;set;}
    }

    public class JobModel {
        public JobModel() {}

        public JobModel(string key, EnqueuedJobDto job) {
            Id = key;
            if (job != null) {
                Name = job.Job.Method.Name;
            }
        }
        public string Name {get;set;}
        public string Id {get;set;}
    }
}
