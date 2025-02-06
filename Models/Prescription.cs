using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
    
    //wlasciwosci nawigacyjne
    public virtual Patient Patient { get; set; } // jeden w strone pacjenta wiele recept
    public virtual Doctor Doctor { get; set; } // jeden w strone lekarza wiele recept
    public virtual ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; } //wiele recept jeden lekarz i jeden pacjent
    
    
}