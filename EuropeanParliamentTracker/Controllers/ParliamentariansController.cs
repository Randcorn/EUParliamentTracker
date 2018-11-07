using System;
using System.Threading.Tasks;
using EuropeanParliamentTracker.Domain;
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

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parliamentarian = await _context.Parliamentarians.Include(x => x.Country).Include(x => x.NationlParty)
                .FirstOrDefaultAsync(m => m.ParliamentarianId == id);
            if (parliamentarian == null)
            {
                return NotFound();
            }

            return View(parliamentarian);
        }
    }
}