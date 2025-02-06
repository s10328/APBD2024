using WebApplication2.DTOs;

namespace WebApplication2.Services;
//pierwsza koncowka - wstawienie nowej recepy
public interface IPrescriptionService
{
    Task<int> AddPrescriptionAsync(PrescriptionRequestDto prescriptionDto); //pierwsza koncowka - wstawienie nowej recepy
    Task<PatientDetailDto> GetPatientDetailsAsync(int patientId); //druga koncowka - dane na temat pacjenta
}

//druga koncowka - dane na temat pacjenta

