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
        public DbSet<DebatePost> DebatePosts { get; set; }
        public DbSet<DebatePostLike> DebatePostLikes { get; set; }
        public DbSet<WatchlistItem> WatchlistItems { get; set; }


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

            modelBuilder.Entity<DebatePost>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.Replies)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DebatePost>()
                .HasOne(p => p.Film)
                .WithMany(f => f.DebatePosts)
                .HasForeignKey(p => p.FilmId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            modelBuilder.Entity<DebatePost>()
                .HasOne(p => p.Series)
                .WithMany(s => s.DebatePosts)
                .HasForeignKey(p => p.SeriesId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.UserId, r.FilmId })
                .IsUnique()
                .HasFilter("[FilmId] IS NOT NULL");

            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.UserId, r.SeriesId })
                .IsUnique()
                .HasFilter("[SeriesId] IS NOT NULL");
            modelBuilder.Entity<DebatePost>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.Replies)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<DebatePostLike>()
                .HasIndex(l => new { l.UserId, l.DebatePostId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DebatePostLike>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DebatePostLike>()
                .HasOne(l => l.DebatePost)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.DebatePostId)
                .OnDelete(DeleteBehavior.Cascade);

            // WatchlistItem – one user can add each film only once
            modelBuilder.Entity<WatchlistItem>()
                .HasIndex(w => new { w.UserId, w.FilmId })
                .IsUnique();

            modelBuilder.Entity<WatchlistItem>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchlistItem>()
                .HasOne(w => w.Film)
                .WithMany()
                .HasForeignKey(w => w.FilmId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
