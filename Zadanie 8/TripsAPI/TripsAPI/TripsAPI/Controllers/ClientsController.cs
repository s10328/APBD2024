namespace TripsAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripsAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TripsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly YourDbContext _context;

        public ClientsController(YourDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var client = await _context.Clients.FindAsync(idClient);
            if (client == null)
            {
                return NotFound();
            }

            var hasTrips = await _context.ClientTrips.AnyAsync(ct => ct.ClientId == idClient);
            if (hasTrips)
            {
                return BadRequest("Client has associated trips and cannot be deleted.");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
