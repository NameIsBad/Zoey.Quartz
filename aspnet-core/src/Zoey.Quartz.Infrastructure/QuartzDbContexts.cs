using Microsoft.EntityFrameworkCore;
using Zoey.Quartz.Domain.Model;

namespace Zoey.Quartz.Infrastructure
{
    public class QuartzDbContexts : DbContext
    {
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<TaskLog> TaskLog { get; set; }
        public virtual DbSet<TaskOptions> TaskOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}