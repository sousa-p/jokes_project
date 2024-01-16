using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jokes_project.Data;
using jokes_project.Models;
using Microsoft.AspNetCore.Authorization;

namespace jokes_project.Controllers
{
    public class JokeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Joke
        public async Task<IActionResult> Index()
        {
            return View(await _context.JokeViewModel.ToListAsync());
        }

        // GET: Joke/Search
        public IActionResult Search()
        {
            return View();
        }

        // GET: Joke/SearchResults
        public async Task<IActionResult> SearchResults(string searchPhrase)
        {
            string cleanedSearch = searchPhrase.ToLower().Trim();
            return View("Index", await _context.JokeViewModel.Where(j => j.Question.ToLower().Contains(cleanedSearch) || j.Answer.ToLower().Contains(cleanedSearch)).ToArrayAsync());
        }

        // GET: Joke/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokeViewModel = await _context.JokeViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jokeViewModel == null)
            {
                return NotFound();
            }

            return View(jokeViewModel);
        }

        // GET: Joke/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Joke/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] JokeViewModel jokeViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jokeViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jokeViewModel);
        }

        // GET: Joke/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokeViewModel = await _context.JokeViewModel.FindAsync(id);
            if (jokeViewModel == null)
            {
                return NotFound();
            }
            return View(jokeViewModel);
        }

        // POST: Joke/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] JokeViewModel jokeViewModel)
        {
            if (id != jokeViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jokeViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeViewModelExists(jokeViewModel.Id))
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
            return View(jokeViewModel);
        }

        // GET: Joke/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokeViewModel = await _context.JokeViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jokeViewModel == null)
            {
                return NotFound();
            }

            return View(jokeViewModel);
        }

        // POST: Joke/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jokeViewModel = await _context.JokeViewModel.FindAsync(id);
            if (jokeViewModel != null)
            {
                _context.JokeViewModel.Remove(jokeViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeViewModelExists(int id)
        {
            return _context.JokeViewModel.Any(e => e.Id == id);
        }
    }
}
