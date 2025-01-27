﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectX.Models;

namespace projectX.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly AppDbContext _context;

        public ScreeningsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var screenings = _context.Screenings
                .Include(s => s.ScreeningMovies)
                    .ThenInclude(sm => sm.Movie)
                .Include(s => s.ScreeningCinemas)
                    .ThenInclude(sc => sc.Cinema)
                .Include(s => s.Tickets);
            return View(await screenings.ToListAsync());
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ScreeningTime")] Screening screening, int[] selectedCinemas)
        {
            if (ModelState.IsValid)
            {
                foreach (var cinemaId in selectedCinemas)
                {
                    var screeningCinema = new ScreeningCinemas
                    {
                        CinemaId = cinemaId,
                        Screening = screening
                    };
                    _context.Add(screeningCinema);
                }

                _context.Add(screening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns();
            return View(screening);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.ScreeningCinemas)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (screening == null)
            {
                return NotFound();
            }

            var selectedCinemaIds = screening.ScreeningCinemas.Select(sc => sc.CinemaId).ToList();
            PopulateDropdowns(selectedCinemaIds);
            return View(screening);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ScreeningTime")] Screening screening, int[] selectedCinemas)
        {

            if (id != screening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(screening);

                    var existingScreeningCinemas = _context.ScreeningCinemas
                        .Where(sc => sc.ScreeningId == id).ToList();

                    if (existingScreeningCinemas.Any())
                    {
                        _context.ScreeningCinemas.RemoveRange(existingScreeningCinemas);
                        await _context.SaveChangesAsync();
                    }


                    foreach (var cinemaId in selectedCinemas)
                    {
                        var screeningCinema = new ScreeningCinemas
                        {
                            ScreeningId = id,
                            CinemaId = cinemaId
                        };
                        _context.Add(screeningCinema);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreeningExists(screening.Id))
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
            PopulateDropdowns(selectedCinemas);
            return View(screening);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screening = await _context.Screenings
                .Include(s => s.ScreeningMovies)
                    .ThenInclude(sm => sm.Movie)
                .Include(s => s.ScreeningCinemas)
                    .ThenInclude(sc => sc.Cinema)
                .Include(s => s.Tickets)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (screening == null)
            {
                return NotFound();
            }

            return View(screening);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screening = await _context.Screenings.FindAsync(id);

            var screeningCinemas = _context.ScreeningCinemas.Where(sc => sc.ScreeningId == id);
            _context.ScreeningCinemas.RemoveRange(screeningCinemas);

            _context.Screenings.Remove(screening);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns(IEnumerable<int>? selectedCinemaIds = null)
        {
            var cinemas = _context.Cinemas?.ToList() ?? new List<Cinema>();
            ViewData["Cinemas"] = new MultiSelectList(cinemas, "Id", "Name", selectedCinemaIds);
        }

        private bool ScreeningExists(int id)
        {
            return _context.Screenings.Any(e => e.Id == id);
        }
    }
}