using API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GAE.AIQ.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        private IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Create relation into tables
            modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(m => m.Movies)
            .UsingEntity<MovieActor>(
                x => x
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId),
                x => x
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId),
                x => {
                    x.Property(ma => ma.PublicationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    x.HasKey(m => new { m.ActorId, m.MovieId });
                });
        }

        
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
    }
}
