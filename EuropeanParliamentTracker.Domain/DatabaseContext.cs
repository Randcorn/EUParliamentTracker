using EuropeanParliamentTracker.Domain.Models;
using EuropeanParliamentTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EuropeanParliamentTracker.Domain
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<NationalParty> NationalParties { get; set; }
        public DbSet<Parliamentarian> Parliamentarians { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<PoliticalGroup> PoliticalGroups { get; set; }
        public DbSet<VoteResult> VoteResults { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<NationalParty>()
                .HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);*/

            modelBuilder.Entity<VoteResult>()
                .HasOne(x => x.Parliamentarian)
                .WithMany()
                .HasForeignKey(x => x.ParliamentarianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VoteResult>()
                .HasOne(x => x.Vote)
                .WithMany()
                .HasForeignKey(x => x.VoteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Parliamentarian>()
                .HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Parliamentarian>()
                .HasOne(x => x.NationalParty)
                .WithMany()
                .HasForeignKey(x => x.NationalPartyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
