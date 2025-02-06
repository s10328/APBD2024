using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class Doctor
{
    [Key] //oznaczenie klucza glownego
    public int IdDoctor { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)] 
    public string LastName { get; set; }
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }
    
    //wlasciwosci nawigacyjne jeden doktor wiele recept
    public virtual ICollection<Prescription> Prescriptions { get; set; }
}