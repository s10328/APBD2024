using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripsAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TripsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly YourDbContext _context;

        public TripsController(YourDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var trips = await _context.Trips
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new 
                {
                    t.Name,
                    t.Description,
                    t.DateFrom,
                    t.DateTo,
                    t.MaxPeople,
                    Countries = t.TripCountries.Select(tc => new { tc.Country.Name }),
                    Clients = t.ClientTrips.Select(ct => new { ct.Client.FirstName, ct.Client.LastName })
                })
                .ToListAsync();

            var totalTrips = await _context.Trips.CountAsync();
            var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

            return Ok(new 
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = totalPages,
                Trips = trips
            });
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] ClientTripDto clientTripDto)
        {
            if (await _context.Clients.AnyAsync(c => c.Pesel == clientTripDto.Pesel))
            {
                return BadRequest("Client with the given PESEL already exists.");
            }

            if (await _context.ClientTrips.AnyAsync(ct => ct.Client.Pesel == clientTripDto.Pesel && ct.TripId == idTrip))
            {
                return BadRequest("Client is already registered for this trip.");
            }

            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null || trip.DateFrom <= DateTime.Now)
            {
                return BadRequest("Trip does not exist or has already started.");
            }

            var client = new Client
            {
                FirstName = clientTripDto.FirstName,
                LastName = clientTripDto.LastName,
                Email = clientTripDto.Email,
                Telephone = clientTripDto.Telephone,
                Pesel = clientTripDto.Pesel
            };

            var clientTrip = new ClientTrip
            {
                TripId = idTrip,
                Client = client,
                PaymentDate = clientTripDto.PaymentDate,
                RegisteredAt = DateTime.Now
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
