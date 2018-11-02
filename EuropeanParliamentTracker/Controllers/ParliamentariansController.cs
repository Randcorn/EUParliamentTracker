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
    }
}