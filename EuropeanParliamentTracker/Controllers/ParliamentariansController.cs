using System;
using System.Linq;
using System.Threading.Tasks;
using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.ViewModels;
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
            return View(await _context.Parliamentarians.Include(x => x.Country).Include(x => x.NationalParty).ToListAsync());
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parliamentarian = await _context.Parliamentarians.Include(x => x.Country).Include(x => x.NationalParty)
                .FirstOrDefaultAsync(m => m.ParliamentarianId == id);
            if (parliamentarian == null)
            {
                return NotFound();
            }

            var parliamentarianVoteResults = _context.VoteResults.Include(x => x.Vote)
                .Where(m => m.ParliamentarianId == id).ToList();

            var viewModel = new ParliamentarianViewModel
            {
                ParliamentarianId = parliamentarian.ParliamentarianId,
                Firstname = parliamentarian.Firstname,
                Lastname = parliamentarian.Lastname,
                OfficalId = parliamentarian.OfficalId,
                NationlParty = parliamentarian.NationalParty,
                Country = parliamentarian.Country,
                VoteResults = parliamentarianVoteResults
            };

            return View(viewModel);
        }
    }
}