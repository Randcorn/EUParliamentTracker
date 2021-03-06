﻿using EuropeanParliamentTracker.Domain.Entities;
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
        public int VoteNumberOfTheDay { get; set; }
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

        public int NumberOfApproveVotes()
        {
            return VoteResults.Count(x => x.VoteType == VoteType.Approve);
        }

        public int NumberOfRejectVotes()
        {
            return VoteResults.Count(x => x.VoteType == VoteType.Reject);
        }

        public int NumberOfAbstainVotes()
        {
            return VoteResults.Count(x => x.VoteType == VoteType.Abstain);
        }

        public int NumberOfAbsentVotes()
        {
            return 750 - NumberOfApproveVotes() - NumberOfRejectVotes() - NumberOfAbstainVotes();
        }
    }
}
