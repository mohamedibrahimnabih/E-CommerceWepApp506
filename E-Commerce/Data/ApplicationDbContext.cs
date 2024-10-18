using E_Commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_Commerce.ViewModel;

namespace E_Commerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public ApplicationDbContext()
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true).Build();

            var connection = builder.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connection);
        }
        public DbSet<E_Commerce.ViewModel.ApplicationUserVM> ApplicationUserVM { get; set; } = default!;
        public DbSet<E_Commerce.ViewModel.LoginVM> LoginVM { get; set; } = default!;
    }
}
