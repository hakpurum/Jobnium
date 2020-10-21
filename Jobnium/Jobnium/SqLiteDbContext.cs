using Jobnium.Abstract;
using Jobnium.Model;
using Microsoft.EntityFrameworkCore;

namespace Jobnium
{
    public class SqLiteDbContext : DbContext
    {
        public DbSet<JobLog> JobLog { get; set; }
        public DbSet<JobTable> JobTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($@"Data Source={StaticValues.DbFilePath};");
        }
    }
}
