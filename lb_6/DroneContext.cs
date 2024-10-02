using lb_6;
using Microsoft.EntityFrameworkCore;

namespace FirstEF6App
{
    class DroneContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DronesDB.db");
        }
        public DroneContext() => Database.EnsureCreated();
        public DbSet<Drone> Drones { get; set; }
    }
}
