using EuropeanParliamentTracker.DataIntegrations.Common;
using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EuropeanParliamentTracker.DataIntegrations.ParliamentariansIntegration
{
    public class ParliamentariansIntegration
    {
        private readonly DatabaseContext _context;

        public ParliamentariansIntegration(DatabaseContext context)
        {
            _context = context;
        }

        public async Task IntegrateParliamentariansAsync()
        {
            var meps = await GetDataAsync();
            SaveMeps(meps);
        }

        private void SaveMeps(meps meps)
        {
            foreach (var mep in meps.Items)
            {
                var country = _context.Countries.FirstOrDefault(x => x.Name == mep.country);
                if (country == null)
                {
                    country = new Country
                    {
                        CountryId = Guid.NewGuid(),
                        Name = mep.country
                    };
                    _context.Countries.Add(country);
                }

                var nationalParty = _context.NationalParties.FirstOrDefault(x => x.Name == mep.nationalPoliticalGroup);
                if (nationalParty == null)
                {
                    nationalParty = new NationalParty
                    {
                        NationalPartyId = Guid.NewGuid(),
                        Name = mep.nationalPoliticalGroup
                    };
                    _context.NationalParties.Add(nationalParty);
                }

                var parliamentarian = _context.Parliamentarians.FirstOrDefault(x => x.OfficalId == mep.id);
                if (parliamentarian == null)
                {
                    parliamentarian = new Parliamentarian
                    {
                        Firstname = GetFirstname(mep.fullName),
                        Lastname = GetLastname(mep.fullName),
                        OfficalId = mep.id,
                        Country = country,
                        NationalParty = nationalParty
                    };
                    _context.Parliamentarians.Add(parliamentarian);
                }
                _context.SaveChanges();
            }
        }

        private string GetFirstname(string fullName)
        {
            var allNames = fullName.Split(' ');
            var firstname = string.Empty;
            foreach (var name in allNames)
            {
                if (!char.IsUpper(name[name.Length - 1]))
                {
                    if (!string.IsNullOrEmpty(firstname))
                    {
                        firstname += " ";
                    }
                    firstname += name;
                }
            }
            return firstname;
        }

        private string GetLastname(string fullName)
        {
            var allNames = fullName.Split(' ');
            var lastname = string.Empty;
            foreach (var name in allNames)
            {
                if (char.IsUpper(name[name.Length - 1]))
                {
                    if (!string.IsNullOrEmpty(lastname))
                    {
                        lastname += " ";
                    }
                    lastname += name;
                }
            }
            return lastname;
        }

        private async Task<meps> GetDataAsync()
        {
            var response = await new Client().GetDataStream("http://www.europarl.europa.eu/meps/en/full-list/xml");
            var serializer = new XmlSerializer(typeof(meps));
            return (meps)serializer.Deserialize(response);
        }
    }
}
