using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimalAPI.Data;
using AnimalAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalContext _context;

        public AnimalsController(AnimalContext context)
        {
            _context = context;
        }

        // GET: api/animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimals([FromQuery] string orderBy = "name")
        {
            IQueryable<Animal> query = _context.Animals;

            switch (orderBy.ToLower())
            {
                case "name":
                    query = query.OrderBy(a => a.Name);
                    break;
                case "description":
                    query = query.OrderBy(a => a.Description);
                    break;
                case "category":
                    query = query.OrderBy(a => a.Category);
                    break;
                case "area":
                    query = query.OrderBy(a => a.Area);
                    break;
                default:
                    query = query.OrderBy(a => a.Name);
                    break;
            }

            return await query.ToListAsync();
        }

        // POST: api/animals
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(Animal animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimals), new { id = animal.IdAnimal }, animal);
        }

        // PUT: api/animals/{idAnimal}
        [HttpPut("{idAnimal}")]
        public async Task<IActionResult> PutAnimal(int idAnimal, Animal animal)
        {
            if (idAnimal != animal.IdAnimal)
            {
                return BadRequest();
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(idAnimal))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/animals/{idAnimal}
        [HttpDelete("{idAnimal}")]
        public async Task<IActionResult> DeleteAnimal(int idAnimal)
        {
            var animal = await _context.Animals.FindAsync(idAnimal);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.IdAnimal == id);
        }
    }
}
