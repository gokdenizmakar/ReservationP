using Microsoft.EntityFrameworkCore;

namespace ReservationP.Models
{
    public class RDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=ReservationDb; trusted_connection = true");
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ReservationM> Reservations { get; set; }
    }
}
