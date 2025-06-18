using FirstTask1.Dto;
using FirstTask1.Model;
using Microsoft.EntityFrameworkCore;

namespace FirstTask1.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContextEF(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User").HasKey(u => u.Id);
        }
        public DbSet<FirstTask1.Model.User> User { get; set; } = default!;
    }
}
