using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    
    //wlasciwosc nawigacyjna do tabeli laczacej Prescription_Medicament
    public virtual ICollection<Prescription_Medicament> Prescriptions_Medicaments { get; set; }
}