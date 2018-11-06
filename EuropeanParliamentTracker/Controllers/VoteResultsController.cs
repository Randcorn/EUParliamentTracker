using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;

namespace EuropeanParliamentTracker.Controllers
{
    public class VoteResultsController : Controller
    {
        private readonly DatabaseContext _context;

        public VoteResultsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: VoteResults
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.VoteResults.Include(v => v.Parliamentarian).Include(v => v.Vote);
            return View(await databaseContext.ToListAsync());
        }

        // GET: VoteResults/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteResult = await _context.VoteResults
                .Include(v => v.Parliamentarian)
                .Include(v => v.Vote)
                .FirstOrDefaultAsync(m => m.VoteResultId == id);
            if (voteResult == null)
            {
                return NotFound();
            }

            return View(voteResult);
        }

        // GET: VoteResults/Create
        public IActionResult Create()
        {
            ViewData["ParliamentarianId"] = new SelectList(_context.Parliamentarians, "ParliamentarianId", "ParliamentarianId");
            ViewData["VoteId"] = new SelectList(_context.Votes, "VoteId", "VoteId");
            return View();
        }

        // POST: VoteResults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoteResultId,VoteType,ParliamentarianId,VoteId")] VoteResult voteResult)
        {
            if (ModelState.IsValid)
            {
                voteResult.VoteResultId = Guid.NewGuid();
                _context.Add(voteResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParliamentarianId"] = new SelectList(_context.Parliamentarians, "ParliamentarianId", "ParliamentarianId", voteResult.ParliamentarianId);
            ViewData["VoteId"] = new SelectList(_context.Votes, "VoteId", "VoteId", voteResult.VoteId);
            return View(voteResult);
        }

        // GET: VoteResults/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteResult = await _context.VoteResults.FindAsync(id);
            if (voteResult == null)
            {
                return NotFound();
            }
            ViewData["ParliamentarianId"] = new SelectList(_context.Parliamentarians, "ParliamentarianId", "ParliamentarianId", voteResult.ParliamentarianId);
            ViewData["VoteId"] = new SelectList(_context.Votes, "VoteId", "VoteId", voteResult.VoteId);
            return View(voteResult);
        }

        // POST: VoteResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VoteResultId,VoteType,ParliamentarianId,VoteId")] VoteResult voteResult)
        {
            if (id != voteResult.VoteResultId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voteResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteResultExists(voteResult.VoteResultId))
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
            ViewData["ParliamentarianId"] = new SelectList(_context.Parliamentarians, "ParliamentarianId", "ParliamentarianId", voteResult.ParliamentarianId);
            ViewData["VoteId"] = new SelectList(_context.Votes, "VoteId", "VoteId", voteResult.VoteId);
            return View(voteResult);
        }

        // GET: VoteResults/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteResult = await _context.VoteResults
                .Include(v => v.Parliamentarian)
                .Include(v => v.Vote)
                .FirstOrDefaultAsync(m => m.VoteResultId == id);
            if (voteResult == null)
            {
                return NotFound();
            }

            return View(voteResult);
        }

        // POST: VoteResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var voteResult = await _context.VoteResults.FindAsync(id);
            _context.VoteResults.Remove(voteResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteResultExists(Guid id)
        {
            return _context.VoteResults.Any(e => e.VoteResultId == id);
        }
    }
}
