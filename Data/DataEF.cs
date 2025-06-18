using Microsoft.EntityFrameworkCore;
using ProductTask.Model;
using System.Data;

namespace ProductTask.Data
{
    public class DataEF : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<ProductTask.Model.Users> Users { get; set; } = default!;
        public DbSet<Product> products { get; set; }
        public DbSet<ProductCategories> categories { get; set; }

        public DbSet<Users> users { get; set; }

        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetailes> OrderDetailes { get; set; }

        public DbSet<CheckoutForm> checkoutForms { get; set; }

        public DataEF(IConfiguration configuration)
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
            modelBuilder.Entity<Product>().ToTable("Product").HasKey(p => p.Id);

            modelBuilder
                .Entity<ProductCategories>()
                .ToTable("ProductCategories")
                .HasKey(ct => ct.CategoryId); // Set TypeID as the key

            modelBuilder.Entity<ProductCategories>().Property(ct => ct.CategoryId).ValueGeneratedNever();
            modelBuilder.Entity<ProductCategories>().HasData(
                new ProductCategories { CategoryId = 1, CategoryName = "Electronics" },
                new ProductCategories { CategoryId = 2, CategoryName = "Home & Kitchen" },
                new ProductCategories { CategoryId = 3, CategoryName = "Clothing" },
                new ProductCategories { CategoryId = 4, CategoryName = "Beauty & Personal Care" },
                new ProductCategories { CategoryId = 5, CategoryName = "Digital Products" }
            );
            modelBuilder.Entity<OrderDetailes>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<CheckoutForm>(entity =>
            {
                entity.ToTable("checkoutForms2");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(50);
                entity.Property(e => e.State).IsRequired().HasMaxLength(50);
                entity.Property(e => e.City).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Address1).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address2).HasMaxLength(200);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);

                entity.OwnsOne(e => e.CardInfo, card =>
                {
                    card.Property(c => c.CardHolderName).HasMaxLength(100).IsRequired(false);
                    card.Property(c => c.CardNumber).HasMaxLength(25).IsRequired(false);
                    card.Property(c => c.ExpirationDate).HasMaxLength(7).IsRequired(false);
                    card.Property(c => c.CVV).HasMaxLength(4).IsRequired(false);
                });
            });


        }


    }

}

