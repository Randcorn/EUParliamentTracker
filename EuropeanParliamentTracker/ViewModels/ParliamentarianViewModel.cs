using EuropeanParliamentTracker.Domain.Entities;
using EuropeanParliamentTracker.Domain.Models;
using System;
using System.Collections.Generic;

namespace EuropeanParliamentTracker.ViewModels
{
    public class ParliamentarianViewModel
    {
        public Guid ParliamentarianId { get; set; }
        public string OfficalId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        
        public Country Country { get; set; }
        public NationalParty NationlParty { get; set; }
        public List<VoteResult> VoteResults { get; set; }
    }
}
