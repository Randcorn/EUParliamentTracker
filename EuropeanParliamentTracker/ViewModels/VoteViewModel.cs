using EuropeanParliamentTracker.Domain.Entities;
using EuropeanParliamentTracker.Domain.Enums;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EuropeanParliamentTracker.ViewModels
{
    public class VoteViewModel
    {
        public Guid VoteId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }
        public List<VoteResult> VoteResults { get; set; }

        public VoteType VoteOutcome()
        {
            var approvedVoteResults = VoteResults.Count(x => x.VoteType == VoteType.Approve);
            var rejectedVoteResults = VoteResults.Count(x => x.VoteType == VoteType.Reject);
            if(approvedVoteResults > rejectedVoteResults)
            {
                return VoteType.Approve;
            }
            return VoteType.Reject;
        }
    }
}
