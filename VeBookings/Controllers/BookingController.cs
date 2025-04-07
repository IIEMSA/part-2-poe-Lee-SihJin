using Microsoft.AspNetCore.Mvc;
using VeBookings.Models;
using Microsoft.EntityFrameworkCore;

namespace VeBookings.Controllers
{
    public class BookingController : Controller
    {

        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()

        {
            var bookings = await _context.Booking
                .Include(i => i.Venue)
                .Include(i => i.Event)
                .ToListAsync();

            return View(bookings);
        }
        public IActionResult Create()
        {
            ViewBag.Venues = _context.Venue.ToList();
            ViewBag.Event = _context.Event.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Venues = _context.Venue.ToList();
            ViewBag.Event = _context.Event.ToList();
            return View(booking);

        }

        public async Task<IActionResult> LogDate(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost]

        public async Task<IActionResult> LogDate(int id, Booking model)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null) return NotFound();

            booking.BookingDate = model.BookingDate;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }
        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);


            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
        


    }

}
