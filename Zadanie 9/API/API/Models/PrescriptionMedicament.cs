namespace API.Models;

public class PrescriptionMedicament
{
    public int PrescriptionId { get; set; }
    public virtual Prescription Prescription { get; set; }
    public int MedicamentId { get; set; }
    public virtual Medicament Medicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}
