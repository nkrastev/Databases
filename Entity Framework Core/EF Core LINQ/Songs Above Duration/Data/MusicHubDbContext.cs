namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using System;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Set primary key for mapping table
            builder.Entity<SongPerformer>(x =>
            {
                x.HasKey(x => new { x.SongId, x.PerformerId });
            });

            //Enums
            builder
                .Entity<Song>()
                .Property(e => e.Genre)
                .HasConversion(
                    v => v.ToString(),
                    v => (Genre)Enum.Parse(typeof(Genre), v));
        }
    }
}
