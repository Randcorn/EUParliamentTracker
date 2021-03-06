﻿using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Pdf;
using System;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VotesIntegrator
    {
        private readonly DatabaseContext _context;
        private DateTime _dayToImportFor;

        public VotesIntegrator(DatabaseContext context)
        {
            _context = context;
        }

        public void IntegrateVotesForDay(DateTime dayToImportFor)
        {
            _dayToImportFor = dayToImportFor;

            var voteInformationUrl = GetVoteInformationUrl(_dayToImportFor);
            var voteInformationPdfText = PdfHelper.GetTextFromPDF(voteInformationUrl, 2);
            var voteInformationPdf = new VoteInformationPdf(voteInformationPdfText, _dayToImportFor);
            var voteInformationIntegrator = new VoteInformationIntegrator(_context);
            voteInformationIntegrator.IntegrateVoteInformation(voteInformationPdf);
            
            var voteResultUrl = GetVoteResultUrl(_dayToImportFor);
            var voteResultPdfText = PdfHelper.GetTextFromPDF(voteResultUrl, 2);
            var voteResultPdf = new VoteResultPdf(voteResultPdfText, _dayToImportFor);
            var voteResultIntegrator = new VoteResultIntegrator(_context, _dayToImportFor);
            voteResultIntegrator.IntegrateVoteResult(voteResultPdf);
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
