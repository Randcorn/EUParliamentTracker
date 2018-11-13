using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EuropeanParliamentTracker.Domain;
using EuropeanParliamentTracker.Domain.Entities;
using EuropeanParliamentTracker.ViewModels;
using System.Collections.Generic;

namespace EuropeanParliamentTracker.Controllers
{
    public class VotesController : Controller
    {
        private readonly DatabaseContext _context;

        public VotesController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(VoteFilterViewModel viewModel)
        {
            var votes = new List<Vote>();
            if (viewModel.DateToShow == DateTime.MinValue)
            {
                votes = await _context.Votes.ToListAsync();
            }
            else
            {
                votes = await _context.Votes.Where(x => x.Date == viewModel.DateToShow).ToListAsync();
            }

            var voteFilterViewModel = new VoteFilterViewModel()
            {
                Votes = new List<VoteViewModel>(),
                DateToShow = viewModel.DateToShow
            };

            foreach (var vote in votes)
            {
                var voteViewModel = new VoteViewModel
                {
                    VoteId = vote.VoteId,
                    Name = vote.Name,
                    Code = vote.Code,
                    Date = vote.Date,
                    VoteResults = _context.VoteResults.Where(x => x.VoteId == vote.VoteId).Include(x => x.Parliamentarian).ToList()
                };
                voteFilterViewModel.Votes.Add(voteViewModel);
            }

            return View(voteFilterViewModel);
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .FirstOrDefaultAsync(m => m.VoteId == id);
            if (vote == null)
            {
                return NotFound();
            }

            var voteViewModel = new VoteViewModel
            {
                VoteId = vote.VoteId,
                Name = vote.Name,
                Code = vote.Code,
                Date = vote.Date,
                VoteResults = _context.VoteResults.Where(x => x.VoteId == vote.VoteId).Include(x => x.Parliamentarian).ToList()
            };

            return View(voteViewModel);
        }

        // GET: Votes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoteId,Name,Code")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                vote.VoteId = Guid.NewGuid();
                _context.Add(vote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vote);
        }

        // GET: Votes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VoteId,Name,Code")] Vote vote)
        {
            if (id != vote.VoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.VoteId))
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
            return View(vote);
        }

        // GET: Votes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .FirstOrDefaultAsync(m => m.VoteId == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vote = await _context.Votes.FindAsync(id);
            
            var voteResults = _context.VoteResults.Where(x => x.VoteId == vote.VoteId);
            _context.VoteResults.RemoveRange(voteResults);

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteExists(Guid id)
        {
            return _context.Votes.Any(e => e.VoteId == id);
        }
    }
}
