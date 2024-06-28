namespace API.Dtos;

public class PatientResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PrescriptionResponse> Prescriptions { get; set; }
}

public class PrescriptionResponse
{
    public int Id { get;
