using EuropeanParliamentTracker.Domain.Models;
using System.Collections.Generic;

namespace EUParliamentTracker.Application.Interfaces
{
    public interface INationalPartiesRepository
    {
        List<NationalParty> GetNationalParties();
    }
}
