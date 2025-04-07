using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeBookings.Models;

namespace VeBookings.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VenueController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()

        {
            var venues = await _context.Venue.ToListAsync();
            return View(venues);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Venue venues)
        {


            if (ModelState.IsValid)
            {

                _context.Add(venues);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(venues);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var venues = await _context.Venue.FirstOrDefaultAsync(m => m.VenueId == id);


            if (venues == null)
            {
                return NotFound();
            }
            return View(venues);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var venues = await _context.Venue.FindAsync(id);
            _context.Venue.Remove(venues);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueId == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venues = await _context.Venue.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            return View(venues);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Venue venues)
        {
            if (id != venues.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venues);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venues.VenueId))
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

            return View(venues);
        }
    }
    }
