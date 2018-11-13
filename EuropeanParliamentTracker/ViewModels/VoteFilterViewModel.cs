using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EuropeanParliamentTracker.ViewModels
{
    public class VoteFilterViewModel
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateToShow { get; set; }

        public List<VoteViewModel> Votes { get; set; }
    }
}
