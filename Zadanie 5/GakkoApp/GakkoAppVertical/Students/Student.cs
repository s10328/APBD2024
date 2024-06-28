using System.ComponentModel.DataAnnotations;

namespace GakkoAppVertical.Students;

public class Student
{
    public int IdStudent { get; set; }
    [Required]
    [MaxLength(200)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(200)]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MaxLength(200)]
    public string Address { get; set; }
    [Required]
    public int IndexNumber { get; set; }
}