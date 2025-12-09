using movie_service_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace movie_service_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Film <-> Genre many-to-many
            modelBuilder.Entity<Film>()
                .HasMany(f => f.Genre)
                .WithMany(g => g.Films)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmGenre", // naziv join tabele
                    j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                    j => j.HasOne<Film>().WithMany().HasForeignKey("FilmId")
                );

            // Series <-> Genre many-to-many
            modelBuilder.Entity<Series>()
                .HasMany(s => s.Genre)
                .WithMany(g => g.Series)
                .UsingEntity<Dictionary<string, object>>(
                    "SeriesGenre",
                    j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                    j => j.HasOne<Series>().WithMany().HasForeignKey("SeriesId")
                );
        }
    }
}
