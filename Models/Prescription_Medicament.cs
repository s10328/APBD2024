//KROK 1
//MODELS

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models;

public class Prescription_Medicament
{
    [Key]
    [Column(Order = 1)]
    public int IdMedicament { get; set; }
    [Key]
    [Column(Order = 2)]
    public int IdPrescription { get; set; }
    public int Dose { get; set; }
    public int Details { get; set; }
    
    //wlasciwosci nawigacyjne
    // Reprezentuje powiązanie z konkretnym lekiem
    // Relacja: wiele-do-jeden (wiele wpisów w Prescription_Medicament może wskazywać na jeden Medicament)
    public virtual Medicament Medicament { get; set; }
    // Reprezentuje powiązanie z konkretną receptą
    // Relacja: wiele-do-jeden (wiele wpisów w Prescription_Medicament może wskazywać na jedną Prescription)
    public virtual Prescription Prescription { get; set; }
}