using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;
using System;
using System.Linq;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VoteInformationIntegrator
    {
        private readonly DatabaseContext _context;
        private readonly DateTime _dayToImportFor;

        public VoteInformationIntegrator(DatabaseContext context, DateTime dayToImportFor)
        {
            _context = context;
            _dayToImportFor = dayToImportFor;
        }

        public void IntegrateVoteInformation(string pdfText)
        {
            var voteNumber = 1;
            while (true)
            {
                var lengthToNextVote = pdfText.IndexOf(voteNumber + ". ");
                if (lengthToNextVote == -1)
                {
                    break;
                }
                lengthToNextVote += 3;
                pdfText = pdfText.Remove(0, lengthToNextVote);

                var lengthToReport = pdfText.IndexOf("Report");
                var lengthToReccomendation = pdfText.IndexOf("Recommendation");
                var lengthToMotionForAResolution = pdfText.IndexOf("Motion for a resolution");
                var lengthToMotionsForResolutions = pdfText.IndexOf("Motions for resolutions");
                var lengthToCode = pdfText.IndexOf("/" + _dayToImportFor.ToString("yyyy")) - 7;
                var lengthOfName = lengthToReport;
                if (lengthToReport == -1 || (lengthToReport > lengthToReccomendation && lengthToReccomendation != -1))
                {
                    lengthOfName = lengthToReccomendation;
                }
                if (lengthOfName == -1 || (lengthToCode != -1 && lengthToCode < lengthOfName))
                {
                    lengthOfName = lengthToCode;
                }
                if (lengthOfName == -1 || (lengthToMotionForAResolution != -1 && lengthToMotionForAResolution < lengthOfName))
                {
                    lengthOfName = lengthToMotionForAResolution;
                }
                if (lengthOfName == -1 || (lengthToMotionsForResolutions != -1 && lengthToMotionsForResolutions < lengthOfName))
                {
                    lengthOfName = lengthToMotionsForResolutions;
                }
                var voteName = pdfText.Substring(0, lengthOfName);
                voteName = voteName.Replace("\n", "");

                var endOfCode = pdfText.IndexOf("/" + _dayToImportFor.ToString("yyyy"));
                var code = pdfText.Substring(endOfCode - 7, 12);

                if (_context.Votes.Any(x => x.Code == code))
                {
                    continue;
                }

                var vote = new Vote
                {
                    VoteId = Guid.NewGuid(),
                    Name = voteName,
                    Code = code,
                    Date = _dayToImportFor
                };
                _context.Add(vote);
                voteNumber++;
            }
            _context.SaveChanges();
        }
    }
}
