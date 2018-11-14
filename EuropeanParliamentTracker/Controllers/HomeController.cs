using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EuropeanParliamentTracker.ViewModels;
using EuropeanParliamentTracker.Domain;
using System.Linq;
using System.Threading.Tasks;
using EuropeanParliamentTracker.DataIntegrations.ParliamentariansIntegration;
using EuropeanParliamentTracker.DataIntegrations.CountriesIntegration;
using EuropeanParliamentTracker.DataIntegrations.VotesIntegration;

namespace EuropeanParliamentTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ParliamentariansIntegration _parliamentariansIntegration;
        private readonly CountriesIntegration _countriesIntegration;
        private readonly VotesIntegrator _votesIntegration;

        public HomeController(DatabaseContext context, ParliamentariansIntegration parliamentariansIntegration, CountriesIntegration countriesIntegration, VotesIntegrator votesIntegration)
        {
            _context = context;
            _parliamentariansIntegration = parliamentariansIntegration;
            _countriesIntegration = countriesIntegration;
            _votesIntegration = votesIntegration;
        }

        public IActionResult Index(IntegratorViewModel viewModel)
        {
            return View(viewModel);
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

        public IActionResult RemoveForOneDate(IntegratorViewModel viewModel)
        {
            _context.VoteResults.RemoveRange(_context.VoteResults.Where(x => x.Vote.Date == viewModel.DateToIntegrateFor).ToList());
            _context.Votes.RemoveRange(_context.Votes.Where(x => x.Date == viewModel.DateToIntegrateFor));
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ImportCountries()
        {
            await _countriesIntegration.IntegrateCountries();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ImportParliamentarians()
        {
            await _parliamentariansIntegration.IntegrateParliamentariansAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ReadFromPdf(IntegratorViewModel viewModel)
        {
            _votesIntegration.IntegrateVotesForDay(viewModel.DateToIntegrateFor);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

