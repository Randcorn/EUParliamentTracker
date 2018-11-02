using System;

namespace EuropeanParliamentTracker.Domain.Entities
{
    public class PoliticalGroup
    {
        public Guid PoliticalId { get; set; }
        public string Name { get; set; }
    }
}
