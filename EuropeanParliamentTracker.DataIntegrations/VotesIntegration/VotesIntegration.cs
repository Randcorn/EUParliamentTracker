using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;
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
            /*var voteInformationUrl = GetVoteInformationUrl(dayToImportFor);
            var voteInformationPdfText = PdfHelper.GetTextFromPDF(voteInformationUrl);
            ParseVoteInformationPdf(voteInformationPdfText);*/

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
            var lengthToContentList = pdfText.IndexOf("1. ");
            pdfText = pdfText.Remove(0, lengthToContentList + 3);

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
                var lengthOfName = lengthToReport;
                if (lengthToReport == -1 || (lengthToReport > lengthToReccomendation && lengthToReccomendation != -1))
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
            /*var voteList = pdfText.Split(new string[] { "\n+ \n \n \n" }, StringSplitOptions.None).ToList();
            voteList.RemoveAt(0);
            foreach (var vote in voteList)
            {
                var politicalGroupList = vote.Split(new string[] { "\n \n" }, StringSplitOptions.None);
                foreach (var politicalGroup in politicalGroupList)
                {

                }
            }*/
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
