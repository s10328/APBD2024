namespace Assigment_4.Models;

public class Visit
{
    public int Id { get; set; }
    public DateTime DateOfVisit { get; set; }
    public int AnimalId { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}
