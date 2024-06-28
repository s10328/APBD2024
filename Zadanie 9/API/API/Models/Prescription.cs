namespace API.Models;

public class Prescription
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int PatientId { get; set; }
    public virtual Patient Patient { get; set; }
    public int DoctorId { get; set; }
    public virtual Doctor Doctor { get; set; }
    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}
