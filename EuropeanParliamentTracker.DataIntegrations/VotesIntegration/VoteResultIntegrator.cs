using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;
using EuropeanParliamentTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VoteResultIntegrator
    {
        private readonly DatabaseContext _context;
        private readonly DateTime _dayToImportFor;

        private List<string> noParliamentariansFound;
        private List<string> severalParliamentariansFound;

        public VoteResultIntegrator(DatabaseContext context, DateTime dayToImportFor)
        {
            _context = context;
            _dayToImportFor = dayToImportFor;
        }

        public void IntegrateVoteResult(string pdfText)
        {
            var voteNumber = 1;
            while (true)
            {
                noParliamentariansFound = new List<string>();
                severalParliamentariansFound = new List<string>();

                var lengthToNextVote = pdfText.IndexOf(" " + voteNumber + ". ");
                if (lengthToNextVote == -1)
                {
                    break;
                }
                lengthToNextVote += 3 + voteNumber.ToString().Length;
                pdfText = pdfText.Remove(0, lengthToNextVote);

                var endOfVoteCode = pdfText.IndexOf(" ") + 1;
                var voteCode = pdfText.Substring(0, endOfVoteCode);
                voteCode = voteCode.TrimEnd(' ');

                var vote = _context.Votes.FirstOrDefault(x => x.Code == voteCode);
                if (vote == null)
                {
                    voteNumber++;
                    continue;
                }

                var lengthOfCurrentVoteSection = pdfText.IndexOf(" " + (voteNumber + 1) + ". ");
                if (lengthOfCurrentVoteSection == -1)
                {
                    lengthOfCurrentVoteSection = pdfText.Length;
                }
                var currentVoteSection = pdfText.Substring(0, lengthOfCurrentVoteSection);
                var voteSectionsByApproval = currentVoteSection.Split("\n \n \n").ToList();

                AddVotesFromVoteSection(voteSectionsByApproval[2], vote.VoteId, VoteType.Approve);
                AddVotesFromVoteSection(voteSectionsByApproval[3], vote.VoteId, VoteType.Reject);
                if (voteSectionsByApproval.Count() > 4)
                {
                    AddVotesFromVoteSection(voteSectionsByApproval[4], vote.VoteId, VoteType.Abstain);
                }

                _context.SaveChanges();
                voteNumber++;
            }
        }

        private void AddVotesFromVoteSection(string voteSection, Guid voteId, VoteType voteType)
        {
            var parliamentarianStrings = voteSection.Split(new[] { ", ", "\n \n" }, StringSplitOptions.None).ToList();

            foreach (var parliamentarianString in parliamentarianStrings)
            {
                var parliamentarianName = parliamentarianString;
                if (parliamentarianName.Contains(":"))
                {
                    parliamentarianName = parliamentarianName.Split(": ")[1];
                }
                parliamentarianName = parliamentarianName.Replace("\n", "");

                var parliamentarianId = FindParliamentarianIdFromName(parliamentarianName);

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
            var parliamentarians = _context.Parliamentarians.Where(x => x.Lastname == parliamentarianName.ToUpperInvariant());
            if (parliamentarians.Count() != 1)
            {
                parliamentarians = _context.Parliamentarians.Where(x => x.Lastname + " " + x.Firstname == parliamentarianName.ToUpperInvariant());
                if (parliamentarians.Count() != 1)
                {
                    parliamentarians = _context.Parliamentarians.Where(x => (x.Firstname + " " + x.Lastname).ToUpperInvariant().Contains(parliamentarianName.ToUpperInvariant()));
                }

                var pars = parliamentarians.ToList();

                pars = parliamentarians.ToList();
                if (parliamentarians.Count() != 1)
                {
                    var apa = 2;
                }
            }
            if (parliamentarians.Count() == 0)
            {
                noParliamentariansFound.Add(parliamentarianName);
                return null;
            }
            if (parliamentarians.Count() > 1)
            {
                severalParliamentariansFound.Add(parliamentarianName);
                return null;
            }
            return parliamentarians.FirstOrDefault()?.ParliamentarianId;
        }
    }
}
