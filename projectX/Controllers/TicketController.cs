using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectX.Models;
using System.Net.Sockets;

namespace projectX.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;
        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Screening);
            return View(await tickets.ToListAsync());
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,ScreeningId,MovieId")] Ticket ticket)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == ticket.UserId);
            var screening = _context.Screenings.FirstOrDefault(x => x.Id == ticket.ScreeningId);

            ticket.User = user;
            ticket.Screening = screening;

            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns(ticket.UserId, ticket.ScreeningId);
            return View(ticket);
        }
        private void PopulateDropdowns(object selectedUserId = null, object selectedScreeningId = null)
        {
            var users = _context.Users?.ToList() ?? new List<User>();
            var screenings = _context.Screenings?.ToList() ?? new List<Screening>();

            ViewData["ScreeningId"] = new SelectList(screenings, "Id", "Name", selectedScreeningId);
            ViewData["UserId"] = new SelectList(users, "Id", "Username", selectedUserId);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            PopulateDropdowns(ticket.UserId, ticket.ScreeningId);
            return View(ticket);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,ScreeningId,UserId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            PopulateDropdowns(ticket.UserId, ticket.ScreeningId);
            return View(ticket);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Screening)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
