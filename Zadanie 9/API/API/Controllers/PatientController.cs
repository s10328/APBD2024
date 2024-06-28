namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PatientController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null)
            return NotFound();

        var response = new PatientResponse
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions.OrderBy(pr => pr.DueDate).Select(pr => new PrescriptionResponse
            {
                Id = pr.Id,
                Date = pr.Date,
                DueDate = pr.DueDate,
                Doctor = new DoctorResponse
                {
                    Id = pr.Doctor.Id,
                    FirstName = pr.Doctor.FirstName,
                    LastName = pr.Doctor.LastName
                },
                Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentResponse
                {
                    Id = pm.Medicament.Id,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Description = pm.Description
                }).ToList()
            }).ToList()
        };

        return Ok(response);
    }
}
