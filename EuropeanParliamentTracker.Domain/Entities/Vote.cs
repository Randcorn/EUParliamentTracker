using System;
using System.Collections.Generic;
using System.Text;

namespace EuropeanParliamentTracker.Domain.Entities
{
    public class Vote
    {
        public Guid VoteId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
