using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTOs;
using WebApplication2.Services;
using System.Net.Mime; // Dodajemy do obsługi typów mediów

namespace WebApplication2.Controllers;

/// <summary>
/// Kontroler do zarządzania receptami w systemie
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)] // Określamy, że nasze API produkuje odpowiedzi w formacie JSON
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    /// <summary>
    /// Konstruktor kontrolera recept
    /// </summary>
    /// <param name="prescriptionService">Serwis do obsługi operacji na receptach</param>
    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService ?? throw new ArgumentNullException(nameof(prescriptionService));
    }

    /// <summary>
    /// Dodaje nową receptę do systemu
    /// </summary>
    /// <remarks>
    /// Przykładowe żądanie:
    /// 
    ///     POST /api/Prescription
    ///     {
    ///         "patient": {
    ///             "firstName": "Jan",
    ///             "lastName": "Kowalski",
    ///             "birthDate": "1980-01-01"
    ///         },
    ///         "date": "2024-02-06",
    ///         "dueDate": "2024-03-06",
    ///         "idDoctor": 1,
    ///         "medicaments": [
    ///             {
    ///                 "idMedicament": 1,
    ///                 "dose": 2,
    ///                 "details": "Rano i wieczorem"
    ///             }
    ///         ]
    ///     }
    /// </remarks>
    /// <param name="prescriptionDto">Dane nowej recepty</param>
    /// <returns>ID utworzonej recepty</returns>
    /// <response code="201">Recepta została pomyślnie utworzona</response>
    /// <response code="400">Nieprawidłowe dane wejściowe lub przekroczony limit leków</response>
    /// <response code="404">Nie znaleziono lekarza lub leku o podanym ID</response>
    /// <response code="500">Błąd serwera podczas przetwarzania żądania</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)] // Określamy, że endpoint przyjmuje dane w formacie JSON
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPrescription(PrescriptionRequestDto prescriptionDto)
    {
        if (prescriptionDto == null)
        {
            return BadRequest("Dane recepty nie mogą być puste");
        }

        try
        {
            var prescriptionId = await _prescriptionService.AddPrescriptionAsync(prescriptionDto);
            // Zwracamy Created z pełnym URL do nowo utworzonego zasobu
            return Created($"/api/prescription/{prescriptionId}", new { Id = prescriptionId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
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