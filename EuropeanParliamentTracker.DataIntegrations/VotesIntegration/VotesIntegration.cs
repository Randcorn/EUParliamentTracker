using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;
using EuropeanParliamentTracker.Domain.Enums;
using EuropeanParliamentTracker.Pdf;
using System;
using System.Linq;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VotesIntegration
    {
        private readonly DatabaseContext _context;

        public VotesIntegration(DatabaseContext context)
        {
            _context = context;
        }

        public void IntegrateVotesForDay(DateTime dayToImportFor)
        {
            var voteInformationUrl = GetVoteInformationUrl(dayToImportFor);
            var voteInformationPdfText = PdfHelper.GetTextFromPDF(voteInformationUrl);
            ParseVoteInformationPdf(voteInformationPdfText);

            var voteResultUrl = GetVoteResultUrl(dayToImportFor);
            var voteResultPdfText = PdfHelper.GetTextFromPDF(voteResultUrl);
            ParseVoteResultPdf(voteResultPdfText);
        }

        public void ParseVoteInformationPdf(string pdfText)
        {
            var voteNumber = 1;
            while (true)
            {
                var lengthToNextVote = pdfText.IndexOf(voteNumber + ". ");
                if(lengthToNextVote == -1)
                {
                    break;
                }
                lengthToNextVote += 3;
                pdfText = pdfText.Remove(0, lengthToNextVote);

                var lengthToReport = pdfText.IndexOf("Report");
                var lengthToReccomendation = pdfText.IndexOf("Recommendation");
                var lengthOfName = lengthToReport;
                if(lengthToReport == -1 || (lengthToReport > lengthToReccomendation && lengthToReccomendation != -1))
                {
                    lengthOfName = lengthToReccomendation;
                }
                var voteName = pdfText.Substring(0, lengthOfName);
                voteName = voteName.TrimEnd('\n');
                voteName = voteName.TrimEnd(' ');

                var startOfCode = pdfText.IndexOf("(") + 1;
                var endOfCode = pdfText.IndexOf(")");
                var code = pdfText.Substring(startOfCode, endOfCode - startOfCode);

                var vote = new Vote
                {
                    VoteId = Guid.NewGuid(),
                    Name = voteName,
                    Code = code
                };
                _context.Add(vote);
                voteNumber++;
            }
            _context.SaveChanges();
        }

        public void ParseVoteResultPdf(string pdfText)
        {
            var voteNumber = 1;
            while (true)
            {
                var lengthToNextVote = pdfText.IndexOf(" " + voteNumber + ". ");
                if (lengthToNextVote == -1)
                {
                    break;
                }
                lengthToNextVote += 4;
                pdfText = pdfText.Remove(0, lengthToNextVote);

                var endOfVoteCode = pdfText.IndexOf(" ") + 1;
                var voteCode = pdfText.Substring(0, endOfVoteCode);

                var vote = _context.Votes.FirstOrDefault(x => x.Code == voteCode);
                if(vote == null)
                {
                    continue;
                }

                var lengthOfCurrentVoteSection = pdfText.IndexOf(" " + (voteNumber + 1) + ". ");
                var currentVoteSection = pdfText.Substring(0, lengthOfCurrentVoteSection);
                var voteSectionsByApproval = currentVoteSection.Split("\n \n \n").ToList();

                AddVotesFromVoteSection(voteSectionsByApproval[2], vote.VoteId, VoteType.Approve);
                AddVotesFromVoteSection(voteSectionsByApproval[3], vote.VoteId, VoteType.Reject);
                AddVotesFromVoteSection(voteSectionsByApproval[4], vote.VoteId, VoteType.Abstain);

                voteNumber++;
            }
            _context.SaveChanges();
        }

        private void AddVotesFromVoteSection(string voteSection, Guid voteId, VoteType voteType)
        {
            var parliamentarianStrings = voteSection.Split(new [] { ", ", "\n \n" }, StringSplitOptions.None).ToList();

            foreach(var parliamentarianString in parliamentarianStrings)
            {
                var parliamentarianName = parliamentarianString;
                if (parliamentarianName.Contains(":"))
                {
                    parliamentarianName = parliamentarianName.Split(": ")[1];
                }
                parliamentarianName = parliamentarianName.TrimStart('\n');
                parliamentarianName = parliamentarianName.TrimEnd('\n');

                var parliamentarians = _context.Parliamentarians.Where(x => x.Lastname == parliamentarianName.ToUpperInvariant());
                if(parliamentarians.Count() != 1)
                {
                    continue;
                }

                var voteResult = new VoteResult
                {
                    VoteResultId = Guid.NewGuid(),
                    VoteType = voteType,
                    VoteId = voteId,
                    ParliamentarianId = parliamentarians.First().ParliamentarianId
                };
                _context.Add(voteResult);
            }
        }

        private string GetVoteResultUrl(DateTime dayToImportFor)
        {
            return string.Format(GetBaseVoteUrl(), dayToImportFor.ToString("yyyyMMdd"), "RCV");
        }

        private string GetVoteInformationUrl(DateTime dayToImportFor)
        {
            return string.Format(GetBaseVoteUrl(), dayToImportFor.ToString("yyyyMMdd"), "VOT");
        }

        private string GetBaseVoteUrl()
        {
            return "http://www.europarl.europa.eu/sides/getDoc.do?pubRef=-%2f%2fEP%2f%2fNONSGML%2bPV%2b{0}%2bRES-{1}%2bDOC%2bPDF%2bV0%2f%2fEN&language=EN";
        }
    }
}
