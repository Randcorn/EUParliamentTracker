using EuropeanParliamentTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VoteResultPdf
    {
        public DateTime Date { get; set; }
        private string _pdfText { get; set; }

        private int _currentPosition;
        private int _voteNumber;

        private string _approvedVoteSection;
        private string _rejectedVoteSection;
        private string _abstainedVoteSection;

        public VoteResultPdf(string pdfText, DateTime pdfDate)
        {
            _pdfText = pdfText;
            Date = pdfDate;
            _currentPosition = 0;
            _voteNumber = 0;
        }

        public bool HasMoreVotes()
        {
            var searchString = " " + (_voteNumber + 1) + ". ";
            var lengthToNextVote = _pdfText.IndexOf(searchString, _currentPosition);
            return lengthToNextVote != -1;

        }

        public void GoToNextVote()
        {
            _voteNumber++;
            var searchString = " " + _voteNumber + ". ";
            var lengthToNextVote = _pdfText.IndexOf(searchString, _currentPosition);
            if (lengthToNextVote == -1)
            {
                throw new Exception("There are no more votes to be found");
            }
            lengthToNextVote += searchString.Length;
            _currentPosition = lengthToNextVote;
            SaveNextResultSections();
        }

        public string GetVoteCode()
        {
            var endOfVoteCode = _pdfText.IndexOf(" ", _currentPosition) + 1;
            var voteCode = _pdfText.Substring(_currentPosition, endOfVoteCode - _currentPosition);
            voteCode = voteCode.TrimEnd(' ');
            return voteCode;
        }

        public void SaveNextResultSections()
        {
            var endOfCurrentVoteSection = _pdfText.IndexOf(" " + (_voteNumber + 1) + ". ", _currentPosition);
            if (endOfCurrentVoteSection == -1)
            {
                endOfCurrentVoteSection = _pdfText.Length;
            }
            var currentVoteSection = _pdfText.Substring(_currentPosition, endOfCurrentVoteSection - _currentPosition);
            var voteSectionsByApproval = currentVoteSection.Split("\n \n \n").ToList();

            //TODO: Should have better handling for if noone accepts or rejects
            _approvedVoteSection = voteSectionsByApproval[2];
            _rejectedVoteSection = voteSectionsByApproval[3];
            if (voteSectionsByApproval.Count() > 4)
            {
                _abstainedVoteSection = voteSectionsByApproval[4];
            }
        }
        
        public List<string> GetApprovedVoterNames()
        {
            return GetVoterNames(_approvedVoteSection);
        }

        public List<string> GetRejectedVoterNames()
        {
            return GetVoterNames(_rejectedVoteSection);
        }

        public List<string> GetAbstainedVoterNames()
        {
            return GetVoterNames(_abstainedVoteSection);
        }

        private List<string> GetVoterNames(string voteSection)
        {
            var parliamentarianNames = new List<string>();

            var parliamentarianStrings = voteSection.Split(new[] { ", ", "\n \n" }, StringSplitOptions.None).ToList();
            foreach (var parliamentarianString in parliamentarianStrings)
            {
                var parliamentarianName = parliamentarianString;
                if (parliamentarianName.Contains(":"))
                {
                    parliamentarianName = parliamentarianName.Split(": ")[1];
                }
                parliamentarianName = parliamentarianName.Replace("\n", "");
                parliamentarianName = parliamentarianName.TrimEnd(' ');
                parliamentarianNames.Add(parliamentarianName);
            }
            return parliamentarianNames;
        }
    }
}
