// Controllers/PatientController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebApplication2.DTOs;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    /// <summary>
    /// Kontroler do zarządzania danymi pacjentów
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PatientController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PatientController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        /// <summary>
        /// Pobiera szczegółowe informacje o pacjencie wraz z jego receptami i lekami
        /// </summary>
        /// <param name="id">ID pacjenta</param>
        /// <returns>Szczegółowe informacje o pacjencie</returns>
        /// <response code="200">Znaleziono pacjenta i zwrócono jego dane</response>
        /// <response code="404">Nie znaleziono pacjenta o podanym ID</response>
        /// <response code="500">Błąd serwera podczas przetwarzania żądania</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PatientDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientDetails(int id)
        {
            try
            {
                var patientDetails = await _prescriptionService.GetPatientDetailsAsync(id);
                return Ok(patientDetails);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Wystąpił błąd podczas przetwarzania żądania" });
            }
        }
    }
}