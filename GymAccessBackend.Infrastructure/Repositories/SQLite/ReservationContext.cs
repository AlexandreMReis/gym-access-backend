using Microsoft.EntityFrameworkCore;

namespace GymAccessBackend.Infrastructure.Repositories.SQLite
{
    public class ReservationContext : DbContext
    {
        public DbSet<Core.Models.ReservationModel> Reservations { get; set; }
        public string DbPath { get; }
        public ReservationContext()
        {
            DbPath = "reservations.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
