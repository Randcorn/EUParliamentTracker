using System;
using System.ComponentModel.DataAnnotations;

namespace EuropeanParliamentTracker.ViewModels
{
    public class IntegratorViewModel
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateToIntegrateFor { get; set; }
    }
}
