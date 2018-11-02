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
    }
}