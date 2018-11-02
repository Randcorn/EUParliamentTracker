using EuropeanParliamentTracker.Application.Interfaces;
using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EuropeanParliamentTracker.Application.Repositories
{
    public class NationalPartiesRepository : INationalPartiesRepository
    {
        private DatabaseContext _databaseContext { get; set; }

        public NationalPartiesRepository( DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<NationalParty> GetNationalParties()
        {
            return _databaseContext.NationalParties.ToList();
        }

        public NationalParty GetNationalParty(Guid id)
        {
            return _databaseContext.NationalParties.Find(id);
        }
    }
}
