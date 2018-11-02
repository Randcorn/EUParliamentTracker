using EUParliamentTracker.Application.Interfaces;
using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace EUParliamentTracker.Application.Repositories
{
    public class NationalPartiesRepository
    {
        private DatabaseContext _databaseContext { get; set; }

        public NationalPartiesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<NationalParty> GetNationalParties()
        {
            return _databaseContext.NationalParties.ToList();
        }
    }
}
