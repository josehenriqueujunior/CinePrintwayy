using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinePrintwayy.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<Filme> Filme { get; set; }
        public DbSet<Sessao> Sessao { get; set; }
        public DbSet<Sala> Sala { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });

            modelBuilder.Entity<Sala>().HasData(new Sala { Id = 1, Nome = "Sala 1", QuantidadeAcentos = 50 });
            modelBuilder.Entity<Sala>().HasData(new Sala { Id = 2, Nome = "Sala 2", QuantidadeAcentos = 50 });
        }
    }
}
