using Microsoft.EntityFrameworkCore;
using AnimalAPI.Models;

namespace AnimalAPI.Data
{
    public class AnimalContext : DbContext
    {
        public AnimalContext(DbContextOptions<AnimalContext> options) : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }
    }
}