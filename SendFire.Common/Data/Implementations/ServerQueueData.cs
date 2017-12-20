using Microsoft.EntityFrameworkCore;
using SendFire.Common.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendFire.Common.Data.Implementations
{
    public class ServerQueueData
    {
        SendFireContext _dbContext;
        public ServerQueueData(SendFireContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(ServerQueue queue)
        {
             _dbContext.Add(queue);
        }
        
        public void Delete(string queue)
        {
            var serverQueue = _dbContext.ServerQueues.SingleOrDefault(x => x.Queue == queue); 

            if (serverQueue != null)
            {
                _dbContext.ServerQueues.Remove(serverQueue);
                _dbContext.SaveChanges();
            }
        }

        public List<ServerQueue> FindAll()
        {
            return _dbContext.ServerQueues.ToList();
        }
    }
}
