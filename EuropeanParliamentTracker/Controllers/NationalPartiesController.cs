using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EuropeanParliamentTracker.Database;
using EuropeanParliamentTracker.Models;

namespace EuropeanParliamentTracker.Controllers
{
    public class NationalPartiesController : Controller
    {
        private readonly DatabaseContext _context;

        public NationalPartiesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: NationalParties
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.NationalPartys;
            return View(await databaseContext.ToListAsync());
        }

        // GET: NationalParties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nationalParty = await _context.NationalPartys
                .FirstOrDefaultAsync(m => m.NationalPartyId == id);
            if (nationalParty == null)
            {
                return NotFound();
            }

            return View(nationalParty);
        }

        // GET: NationalParties/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id");
            return View();
        }

        // POST: NationalParties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CountryId")] NationalParty nationalParty)
        {
            if (ModelState.IsValid)
            {
                nationalParty.NationalPartyId = Guid.NewGuid();
                _context.Add(nationalParty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nationalParty);
        }

        // GET: NationalParties/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nationalParty = await _context.NationalPartys.FindAsync(id);
            if (nationalParty == null)
            {
                return NotFound();
            }
            return View(nationalParty);
        }

        // POST: NationalParties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,CountryId")] NationalParty nationalParty)
        {
            if (id != nationalParty.NationalPartyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nationalParty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NationalPartyExists(nationalParty.NationalPartyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nationalParty);
        }

        // GET: NationalParties/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nationalParty = await _context.NationalPartys
                .FirstOrDefaultAsync(m => m.NationalPartyId == id);
            if (nationalParty == null)
            {
                return NotFound();
            }

            return View(nationalParty);
        }

        // POST: NationalParties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var nationalParty = await _context.NationalPartys.FindAsync(id);
            _context.NationalPartys.Remove(nationalParty);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NationalPartyExists(Guid id)
        {
            return _context.NationalPartys.Any(e => e.NationalPartyId == id);
        }
    }
}
