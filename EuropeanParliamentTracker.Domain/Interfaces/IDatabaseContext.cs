using EuropeanParliamentTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EuropeanParliamentTracker.Domain.Interfaces
{
    public interface IDatabaseContext
    {
        DbSet<Country> Countries { get; set; }
        DbSet<NationalParty> NationalParties { get; set; }
        DbSet<Parliamentarian> Parliamentarians { get; set; }
    }
}
