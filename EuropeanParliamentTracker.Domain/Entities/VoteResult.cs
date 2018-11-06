using EuropeanParliamentTracker.Domain.Enums;
using EuropeanParliamentTracker.Domain.Models;
using System;

namespace EuropeanParliamentTracker.Domain.Entities
{
    public class VoteResult
    {
        public Guid VoteResultId { get; set; }
        public VoteType VoteType { get; set; }

        public Guid ParliamentarianId { get; set; }
        public Parliamentarian Parliamentarian { get; set; }

        public Guid VoteId { get; set; }
        public Vote Vote { get; set; }
    }
}
