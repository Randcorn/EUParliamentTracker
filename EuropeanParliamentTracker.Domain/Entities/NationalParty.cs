﻿using System;

namespace EuropeanParliamentTracker.Domain.Models
{
    public class NationalParty
    {
        public Guid NationalPartyId { get; set; }
        public string Name { get; set; }

        /*public Guid CountryId { get; set; }
        public Country Country { get; set; }*/
    }
}
