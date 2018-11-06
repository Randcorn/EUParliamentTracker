using EuropeanParliamentTracker.Domain.Entities;
using System;
using System.Collections.Generic;

namespace EuropeanParliamentTracker.ViewModels
{
    public class VoteViewModel
    {
        public Guid VoteId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<VoteResult> VoteResults { get; set; }
    }
}
