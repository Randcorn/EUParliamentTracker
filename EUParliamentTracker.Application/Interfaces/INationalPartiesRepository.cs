using EuropeanParliamentTracker.Domain.Models;
using System;
using System.Collections.Generic;

namespace EuropeanParliamentTracker.Application.Interfaces
{
    public interface INationalPartiesRepository
    {
        List<NationalParty> GetNationalParties();
        NationalParty GetNationalParty(Guid id);
    }
}
