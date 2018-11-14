using System;

namespace EuropeanParliamentTracker.DataIntegrations.VotesIntegration
{
    public class VoteInformationPdf
    {
        public DateTime Date { get; set; }
        private string _pdfText { get; set; }

        private int _currentPosition;
        private int _voteNumber;

        public VoteInformationPdf(string pdfText, DateTime pdfDate)
        {
            _pdfText = pdfText;
            Date = pdfDate;
            _currentPosition = 0;
            _voteNumber = 0;
        }

        public bool HasMoreVotes()
        {
            var searchString = (_voteNumber + 1) + ". ";
            var lengthToNextVote = _pdfText.IndexOf(searchString, _currentPosition);
            return lengthToNextVote != -1;
            
        }

        public void GoToNextVote()
        {
            _voteNumber++;
            var searchString = _voteNumber + ". ";
            var lengthToNextVote = _pdfText.IndexOf(searchString, _currentPosition);
            if (lengthToNextVote == -1)
            {
                throw new Exception("There are no more votes to be found");
            }
            lengthToNextVote += searchString.Length;
            _currentPosition = lengthToNextVote;
        }

        public string GetVoteName()
        {
            var lengthToReport = _pdfText.IndexOf("Report", _currentPosition);
            var lengthToReccomendation = _pdfText.IndexOf("Recommendation", _currentPosition);
            var lengthToMotionForAResolution = _pdfText.IndexOf("Motion for a resolution", _currentPosition);
            var lengthToMotionsForResolutions = _pdfText.IndexOf("Motions for resolutions", _currentPosition);
            var lengthToCode = _pdfText.IndexOf("/" + Date.ToString("yyyy"), _currentPosition) - 7;
            var lengthOfName = lengthToReport;
            if (lengthToReport < 0 || (lengthToReccomendation > 0 && lengthToReport > lengthToReccomendation))
            {
                lengthOfName = lengthToReccomendation;
            }
            if (lengthOfName < 0 || (lengthToCode > 0 && lengthToCode < lengthOfName))
            {
                lengthOfName = lengthToCode;
            }
            if (lengthOfName < 0 || (lengthToMotionForAResolution > 0 && lengthToMotionForAResolution < lengthOfName))
            {
                lengthOfName = lengthToMotionForAResolution;
            }
            if (lengthOfName < 0 || (lengthToMotionsForResolutions > 0 && lengthToMotionsForResolutions < lengthOfName))
            {
                lengthOfName = lengthToMotionsForResolutions;
            }
            lengthOfName -= _currentPosition;
            var voteName = _pdfText.Substring(_currentPosition, lengthOfName);
            voteName = voteName.Replace("\n", "");
            return voteName;
        }

        public string GetVoteCode()
        {
            var endOfCode = _pdfText.IndexOf("/" + Date.ToString("yyyy"), _currentPosition);
            var code = _pdfText.Substring(endOfCode - 7, 12);
            return code;            
        }
    }
}
