using Dal;
using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    //TPH
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasDiscriminator(s => s.Type);

        modelBuilder.Entity<Person>()
            .Property(s => e.Type)
            .HasMaxLength(200);
            .HasColumnName("PersoneType");

        modelBuilder.Entity<Person>().useTphMappingStrategy();
    }*/

    //TPT
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<>().UseTptMappingStrategy();
    }

    //TPC
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().UseTpcMappingStrategy();
    }*/




}