using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectX.Models;
using System.Net.Sockets;

namespace projectX.Controllers
{
    public class HallsController : Controller
    {
        private readonly AppDbContext _context;

        public HallsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var halls = _context.Halls.Include(h => h.Cinema);
            return View(await halls.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hall hall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hall);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }
            return View(hall);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hall hall)
        {
            var cinema = _context.Cinemas.FirstOrDefault(x => x.Id == hall.CinemaId);
            
            hall.Cinema = cinema;
            if (id != hall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallExists(hall.Id))
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
            return View(hall);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.Include(h => h.Cinema).FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hall = await _context.Halls.FindAsync(id);
            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallExists(int id)
        {
            return _context.Halls.Any(e => e.Id == id);
        }
    }
}    
