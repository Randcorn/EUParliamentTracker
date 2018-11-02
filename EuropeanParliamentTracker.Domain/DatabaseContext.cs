using EuropeanParliamentTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EuropeanParliamentTracker.Domain
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<NationalParty> NationalPartys { get; set; }
        public DbSet<Parliamentarian> Parliamentarians { get; set; }

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

            modelBuilder.Entity<Parliamentarian>()
                .HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Parliamentarian>()
                .HasOne(x => x.NationlParty)
                .WithMany()
                .HasForeignKey(x => x.NationalPartyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
