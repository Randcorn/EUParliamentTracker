using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;
using EuropeanParliamentTracker.Domain.Enums;
using EuropeanParliamentTracker.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VotesIntegration
    {
        private readonly DatabaseContext _context;

        private List<string> noParliamentariansFound;
        private List<string> severalParliamentariansFound;
        private DateTime _dayToImportFor;

        public VotesIntegration(DatabaseContext context)
        {
            _context = context;
        }

        public void IntegrateVotesForDay(DateTime dayToImportFor)
        {
            _dayToImportFor = dayToImportFor;

            var voteInformationUrl = GetVoteInformationUrl(_dayToImportFor);
            var voteInformationPdfText = PdfHelper.GetTextFromPDF(voteInformationUrl);
            ParseVoteInformationPdf(voteInformationPdfText);

            var voteResultUrl = GetVoteResultUrl(_dayToImportFor);
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
                var lengthToCode = pdfText.IndexOf("/" + _dayToImportFor.ToString("yyyy")) - 7;
                var lengthOfName = lengthToReport;
                if(lengthToReport == -1 || (lengthToReport > lengthToReccomendation && lengthToReccomendation != -1))
                {
                    lengthOfName = lengthToReccomendation;
                }
                if(lengthOfName == -1 || (lengthToCode != -1 && lengthToCode < lengthOfName))
                {
                    lengthOfName = lengthToCode;
                }
                var voteName = pdfText.Substring(0, lengthOfName);
                voteName = voteName.Replace("\n", "");

                var startOfCode = pdfText.IndexOf("(") + 1;
                var endOfCode = pdfText.IndexOf(")");
                var code = pdfText.Substring(startOfCode, endOfCode - startOfCode);

                if(_context.Votes.Any(x => x.Code == code))
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

        public void ParseVoteResultPdf(string pdfText)
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
                if(vote == null)
                {
                    voteNumber++;
                    continue;
                }

                var lengthOfCurrentVoteSection = pdfText.IndexOf(" " + (voteNumber + 1) + ". ");
                if(lengthOfCurrentVoteSection == -1)
                {
                    lengthOfCurrentVoteSection = pdfText.Length;
                }
                var currentVoteSection = pdfText.Substring(0, lengthOfCurrentVoteSection);
                var voteSectionsByApproval = currentVoteSection.Split("\n \n \n").ToList();

                AddVotesFromVoteSection(voteSectionsByApproval[2], vote.VoteId, VoteType.Approve);
                AddVotesFromVoteSection(voteSectionsByApproval[3], vote.VoteId, VoteType.Reject);
                if(voteSectionsByApproval.Count() > 4)
                {
                    AddVotesFromVoteSection(voteSectionsByApproval[4], vote.VoteId, VoteType.Abstain);
                }

                _context.SaveChanges();
                voteNumber++;
            }
        }

        private void AddVotesFromVoteSection(string voteSection, Guid voteId, VoteType voteType)
        {
            var parliamentarianStrings = voteSection.Split(new [] { ", ", "\n \n" }, StringSplitOptions.None).ToList();

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
            if(parliamentarians.Count() != 1)
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
