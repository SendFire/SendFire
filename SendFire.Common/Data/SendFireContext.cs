using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SendFire.Common.Data.Models;

namespace SendFire.Common.Data
{
    public class SendFireContext : DbContext
    {
        public SendFireContext(DbContextOptions<SendFireContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SendFire");
        }

        public DbSet<ServerQueue> ServerQueues { get; set; }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SendFireContext>
    {
        public SendFireContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<SendFireContext>();
            var connectionString = configuration.GetConnectionString("SendFireDB");
            builder.UseSqlServer(connectionString);
            return new SendFireContext(builder.Options);
        }
    }
}
