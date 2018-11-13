using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;
using System;
using System.Linq;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VoteInformationIntegrator
    {
        private readonly DatabaseContext _context;
        private VoteInformationPdf _pdf;

        public VoteInformationIntegrator(DatabaseContext context)
        {
            _context = context;
        }

        public void IntegrateVoteInformation(VoteInformationPdf pdf)
        {
            _pdf = pdf;
            
            while (true)
            {
                try
                {
                    pdf.GoToNextVote();
                }
                catch
                {
                    break;
                }

                var voteName = pdf.GetVoteName();
                var voteCode = pdf.GetVoteCode();

                if (_context.Votes.Any(x => x.Code == voteCode))
                {
                    continue;
                }

                var vote = new Vote
                {
                    VoteId = Guid.NewGuid(),
                    Name = voteName,
                    Code = voteCode,
                    Date = pdf.Date
                };
                _context.Add(vote);
            }
            _context.SaveChanges();
        }
    }
}
