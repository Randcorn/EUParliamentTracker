using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;
using EuropeanParliamentTracker.Domain.Enums;
using EuropeanParliamentTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VoteResultIntegrator
    {
        private readonly DatabaseContext _context;
        private readonly DateTime _dayToImportFor;

        private List<Vote> _votes;
        private List<Parliamentarian> _parliamentarians;

        public VoteResultIntegrator(DatabaseContext context, DateTime dayToImportFor)
        {
            _context = context;
            _dayToImportFor = dayToImportFor;
            _votes = _context.Votes.Where(x => x.Date == dayToImportFor).ToList();

            _parliamentarians = _context.Parliamentarians.ToList();
        }

        public void IntegrateVoteResult(VoteResultPdf pdf)
        {
            while (pdf.HasMoreVotes())
            {
                pdf.GoToNextVote();

                var voteCode = pdf.GetVoteCode();
                var vote = _votes.FirstOrDefault(x => x.Code == voteCode);
                if (vote == null)
                {
                    continue;
                }

                var approvedVoterNames = pdf.GetApprovedVoterNames();
                var rejectedVoterNames = pdf.GetRejectedVoterNames();
                var abstainedVoterNames = pdf.GetAbstainedVoterNames();

                IntegrateResultType(approvedVoterNames, vote.VoteId, VoteType.Approve);
                IntegrateResultType(rejectedVoterNames, vote.VoteId, VoteType.Reject);
                IntegrateResultType(abstainedVoterNames, vote.VoteId, VoteType.Abstain);

                _context.SaveChanges();
            }
        }

        private void IntegrateResultType(List<string> voterNames, Guid voteId, VoteType voteType)
        {
            foreach (var voterName in voterNames)
            {
                var parliamentarianId = FindParliamentarianIdFromName(voterName);

                if (parliamentarianId == null || _context.VoteResults.Any(x => x.ParliamentarianId == parliamentarianId && x.VoteId == voteId))
                {
                    continue;
                }

                var voteResult = new VoteResult
                {
                    VoteResultId = Guid.NewGuid(),
                    VoteType = voteType,
                    VoteId = voteId,
                    ParliamentarianId = parliamentarianId.Value
                };
                _context.Add(voteResult);
            }
        }

        private Guid? FindParliamentarianIdFromName(string parliamentarianName)
        {
            if(string.IsNullOrWhiteSpace(parliamentarianName))
            {
                return null;
            }
            parliamentarianName = parliamentarianName.Replace("ß", "SS");
            var matchingParliamentarians = _parliamentarians.Where(x => x.Lastname == parliamentarianName.ToUpperInvariant());
            if (matchingParliamentarians.Count() == 1)
            {
                return matchingParliamentarians.First().ParliamentarianId;
            }
            matchingParliamentarians = _parliamentarians.Where(x => (x.Lastname + " " + x.Firstname).ToUpperInvariant() == parliamentarianName.ToUpperInvariant());
            if (matchingParliamentarians.Count() == 1)
            {
                return matchingParliamentarians.First().ParliamentarianId;
            }
            matchingParliamentarians = _parliamentarians.Where(x => (x.Firstname + " " + x.Lastname).ToUpperInvariant().Contains(parliamentarianName.ToUpperInvariant()));
            if (matchingParliamentarians.Count() == 1)
            {
                return matchingParliamentarians.First().ParliamentarianId;
            }
            if(parliamentarianName == "Lange") //TODO: Implement something that can detect this one
            {
                return _parliamentarians.First(x => x.Firstname == "Bernd" && x.Lastname == "LANGE").ParliamentarianId;
            }
            var nameList = parliamentarianName.Split(" ").ToList();
            if(nameList.Any(x => x.Length <= 2))
            {
                nameList.RemoveAll(x => x.Length <= 2);
                var newName = string.Join(" ", nameList);
                return FindParliamentarianIdFromName(newName);
            }
            return null;
        }
    }
}
