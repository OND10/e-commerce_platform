using Email.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Email.API.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<EmailLogger> EmailLoggers { get; set; }
    }

}
