using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Models;
using EuropeanParliamentTracker.Application.Interfaces;

namespace EuropeanParliamentTracker.Controllers
{
    public class NationalPartiesController : Controller
    {
        private readonly INationalPartiesRepository _nationalPartiesRepository;
        private readonly DatabaseContext _context;

        public NationalPartiesController(DatabaseContext context, INationalPartiesRepository nationalPartiesRepository)
        {
            _context = context;
            _nationalPartiesRepository = nationalPartiesRepository;
        }

        // GET: NationalParties
        public IActionResult Index()
        {
            return View(_nationalPartiesRepository.GetNationalParties());
        }

        // GET: NationalParties/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nationalParty = _nationalPartiesRepository.GetNationalParty(id.Value);
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
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nationalParty = _nationalPartiesRepository.GetNationalParty(id.Value);
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

            var nationalParty = await _context.NationalParties
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
        public IActionResult DeleteConfirmed(Guid id)
        {
            var nationalParty = _nationalPartiesRepository.GetNationalParty(id);
            _context.NationalParties.Remove(nationalParty);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool NationalPartyExists(Guid id)
        {
            return _context.NationalParties.Any(e => e.NationalPartyId == id);
        }
    }
}
