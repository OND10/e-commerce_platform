using Microsoft.EntityFrameworkCore;
using Rewards.API.Models;

namespace Rewards.API.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        public DbSet<Reward> Rewards { get; set; }
    }

}
