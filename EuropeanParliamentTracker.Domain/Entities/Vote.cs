using System;

namespace EuropeanParliamentTracker.Domain.Entities
{
    public class Vote
    {
        public Guid VoteId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int VoteNumberOfTheDay { get; set; }
    }
}
