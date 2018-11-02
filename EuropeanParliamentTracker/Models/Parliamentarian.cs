﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EuropeanParliamentTracker.Models
{
    public class Parliamentarian
    {
        public Guid ParliamentarianId { get; set; }
        public int OfficalId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public Guid CountryId { get; set; }
        public Country Country { get; set; }

        public Guid NationalPartyId { get; set; }
        public NationalParty NationlParty { get; set; }
    }
}