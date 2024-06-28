namespace API.Dtos;

public class PrescriptionRequest
{
    public int PatientId { get; set; }
    public string PatientFirstName { get; set; }
    public string PatientLastName { get; set; }
    public DateTime PatientBirthDate { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
}

public class MedicamentDto
{
    public int MedicamentId { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}
