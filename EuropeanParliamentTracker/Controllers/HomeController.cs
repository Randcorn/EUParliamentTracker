using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EuropeanParliamentTracker.ViewModels;
using EuropeanParliamentTracker.Database;
using System.Linq;
using System.Threading.Tasks;
using EuropeanParliamentTracker.Models;
using System;
using System.Net.Http;
using System.IO;
using System.Xml.Serialization;

namespace EuropeanParliamentTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RemoveAll()
        {
            _context.Parliamentarians.RemoveRange(_context.Parliamentarians.ToList());
            _context.NationalPartys.RemoveRange(_context.NationalPartys.ToList());
            _context.Countries.RemoveRange(_context.Countries.ToList());
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ImportData()
        {
            var meps = await GetData();
            SaveMeps(meps);
            return RedirectToAction("Index");
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

                var nationalParty = _context.NationalPartys.FirstOrDefault(x => x.Name == mep.nationalPoliticalGroup);
                if (nationalParty == null)
                {
                    nationalParty = new NationalParty
                    {
                        NationalPartyId = Guid.NewGuid(),
                        Name = mep.nationalPoliticalGroup
                    };
                    _context.NationalPartys.Add(nationalParty);
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
                        NationlParty = nationalParty
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

        private async Task<meps> GetData()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

