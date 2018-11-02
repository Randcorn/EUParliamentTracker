using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EuropeanParliamentTracker.Database;
using EuropeanParliamentTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EuropeanParliamentTracker.Controllers
{
    public class ParliamentariansController : Controller
    {
        private readonly DatabaseContext _context;

        public ParliamentariansController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Parliamentarians.Include(x => x.Country).Include(x => x.NationlParty).ToListAsync());
        }

        public IActionResult RemoveAll()
        {
            _context.Parliamentarians.RemoveRange(_context.Parliamentarians.ToList());
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ImportParliamentarians()
        {
            var meps = await GetMembersInParliament();
            SaveMeps(meps);
            return RedirectToAction("Index");
        }

        private void SaveMeps(meps meps)
        {
            foreach(var mep in meps.Items)
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

                var nationalParty = _context.NationalPartys.FirstOrDefault(x => x.Name == mep.nationalPoliticalGroup);
                if(nationalParty == null)
                {
                    nationalParty = new NationalParty
                    {
                        NationalPartyId = Guid.NewGuid(),
                        Name = mep.nationalPoliticalGroup
                    };
                    _context.NationalPartys.Add(nationalParty);
                }

                var parliamentarian = new Parliamentarian
                {
                    Firstname = GetFirstname(mep.fullName),
                    Lastname = GetLastname(mep.fullName),
                    Country = country,
                    NationlParty = nationalParty
                };
                _context.Parliamentarians.Add(parliamentarian);
            }
            _context.SaveChanges();
        }

        private string GetFirstname(string fullName)
        {
            var allNames = fullName.Split(' ');
            var firstname = string.Empty;
            foreach(var name in allNames)
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
                if(char.IsUpper(name[name.Length - 1]))
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

        private async Task<meps> GetMembersInParliament()
        {
            var response = await GetDataStream();
            var serializer = new XmlSerializer(typeof(meps));
            return (meps)serializer.Deserialize(response);
        }

        private async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            Stream responseData = null;

            try
            {
                var response = await client.GetAsync("http://www.europarl.europa.eu/meps/en/xml.html");
                response.EnsureSuccessStatusCode();
                responseData = await response.Content.ReadAsStreamAsync();
            }
            catch (HttpRequestException e)
            {
                //TODO: Do Something
            }

            client.Dispose();
            return responseData;
        }
    }
}