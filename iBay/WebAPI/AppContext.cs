using Dal;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebAPI
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Charger la configuration depuis appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Utiliser la chaîne de connexion nommée "DefaultConnection"
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        //TPT
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().UseTptMappingStrategy();
            modelBuilder.Entity<Cart>().UseTptMappingStrategy();
        }
    }
}