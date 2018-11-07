using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EuropeanParliamentTracker.ViewModels;
using EuropeanParliamentTracker.Domain;
using System.Linq;
using System.Threading.Tasks;
using EuropeanParliamentTracker.DataIntegrations.ParliamentariansIntegration;
using EuropeanParliamentTracker.DataIntegrations.CountriesIntegration;
using System;
using EuropeanParliamentTracker.DataIntegrations.VotesIntegration;

namespace EuropeanParliamentTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ParliamentariansIntegration _parliamentariansIntegration;
        private readonly CountriesIntegration _countriesIntegration;
        private readonly VotesIntegration _votesIntegration;

        public HomeController(DatabaseContext context, ParliamentariansIntegration parliamentariansIntegration, CountriesIntegration countriesIntegration, VotesIntegration votesIntegration)
        {
            _context = context;
            _parliamentariansIntegration = parliamentariansIntegration;
            _countriesIntegration = countriesIntegration;
            _votesIntegration = votesIntegration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RemoveAll()
        {
            _context.Parliamentarians.RemoveRange(_context.Parliamentarians.ToList());
            _context.NationalParties.RemoveRange(_context.NationalParties.ToList());
            _context.Countries.RemoveRange(_context.Countries.ToList());
            _context.VoteResults.RemoveRange(_context.VoteResults.ToList());
            _context.Votes.RemoveRange(_context.Votes.ToList());
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ImportData()
        {
            await _countriesIntegration.IntegrateCountries();
            await _parliamentariansIntegration.IntegrateParliamentariansAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ReadFromPdf()
        {
            _votesIntegration.IntegrateVotesForDay(new DateTime(2018, 10, 25));
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

