using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    //wlasciwosc nawigacyjna do recept
    public virtual ICollection<Prescription> Prescriptions { get; set; }
}