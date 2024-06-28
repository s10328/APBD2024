namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PrescriptionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequest request)
    {
        if (request.Medicaments.Count > 10)
            return BadRequest("Recepta może obejmować maksymalnie 10 leków.");

        if (request.DueDate < request.Date)
            return BadRequest("DueDate musi być większe lub równe Date.");

        var patient = await _context.Patients.FindAsync(request.PatientId) ?? new Patient
        {
            Id = request.PatientId,
            FirstName = request.PatientFirstName,
            LastName = request.PatientLastName,
            BirthDate = request.PatientBirthDate
        };

        var doctor = await _context.Doctors.FindAsync(request.DoctorId);
        if (doctor == null)
            return NotFound("Lekarz nie istnieje.");

        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            Patient = patient,
            Doctor = doctor,
            PrescriptionMedicaments = new List<PrescriptionMedicament>()
        };

        foreach (var medicamentDto in request.Medicaments)
        {
            var medicament = await _context.Medicaments.FindAsync(medicamentDto.MedicamentId);
            if (medicament == null)
                return NotFound($"Lek o ID {medicamentDto.MedicamentId} nie istnieje.");

            prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                Medicament = medicament,
                Dose = medicamentDto.Dose,
                Description = medicamentDto.Description
            });
        }

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
