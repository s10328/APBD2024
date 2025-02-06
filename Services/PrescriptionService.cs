using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.DTOs;
using WebApplication2.Models;

//DTOs (Data Transfer Objects):
// 
// Stworzyliśmy osobne klasy do transferu danych między klientem a serwerem
// Oddzieliliśmy je od modeli bazy danych, co daje nam większą elastyczność
// Struktura DTOs odzwierciedla format danych, jakie będziemy otrzymywać w żądaniu
// 
// 
// Serwis:
// 
// Implementuje całą logikę biznesową
// Wykonuje walidacje:
// 
// Sprawdza poprawność dat
// Weryfikuje liczbę leków
// Sprawdza istnienie leków w bazie
// 
// 
// Obsługuje logikę wyszukiwania/dodawania pacjenta
// Tworzy nową receptę z powiązaniami do leków
// 
// 
// Kontroler:
// 
// Przyjmuje żądania HTTP
// Deleguje pracę do serwisu
// Obsługuje błędy i zwraca odpowiednie kody HTTP
// Używa wzorca dependency injection dla serwisu
// 
// 
// Obsługa błędów:
// 
// Używamy wyjątków do sygnalizowania błędów biznesowych
// Kontroler przekształca je na odpowiednie odpowiedzi HTTP
// Różne rodzaje błędów są obsługiwane osobno (400 dla błędów walidacji, 500 dla nieoczekiwanych błędów)

namespace WebApplication2.Services;

// Services/PrescriptionService.cs
public class PrescriptionService : IPrescriptionService
{
    private readonly PrescriptionDbContext _context;

    public PrescriptionService(PrescriptionDbContext context)
    {
        _context = context;
    }

    // Istniejąca metoda AddPrescriptionAsync pozostaje bez zmian

    public async Task<PatientDetailDto> GetPatientDetailsAsync(int patientId)
    {
        // Pobieramy pacjenta wraz ze wszystkimi powiązanymi danymi
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(p => p.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(p => p.Prescription_Medicaments)
                    .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null)
        {
            throw new KeyNotFoundException($"Nie znaleziono pacjenta o ID: {patientId}");
        }

        // Mapujemy dane na DTO
        var patientDto = new PatientDetailDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate) // Sortowanie po DueDate
                .Select(p => new PrescriptionDetailDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.Prescription_Medicaments
                        .Select(pm => new MedicamentDetailDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Description = pm.Medicament.Description,
                            Type = pm.Medicament.Type,
                            Dose = pm.Dose,
                            Details = pm.Details
                        }).ToList()
                }).ToList()
        };

        return patientDto;
    }
}

// Services/PrescriptionService.cs
public class PrescriptionService : IPrescriptionService
{
    private readonly PrescriptionDbContext _context;

    public PrescriptionService(PrescriptionDbContext context)
    {
        _context = context;
    }

    // Istniejąca metoda AddPrescriptionAsync pozostaje bez zmian

    public async Task<PatientDetailDto> GetPatientDetailsAsync(int patientId)
    {
        // Pobieramy pacjenta wraz ze wszystkimi powiązanymi danymi
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(p => p.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(p => p.Prescription_Medicaments)
                    .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null)
        {
            throw new KeyNotFoundException($"Nie znaleziono pacjenta o ID: {patientId}");
        }

        // Mapujemy dane na DTO
        var patientDto = new PatientDetailDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate) // Sortowanie po DueDate
                .Select(p => new PrescriptionDetailDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.Prescription_Medicaments
                        .Select(pm => new MedicamentDetailDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Description = pm.Medicament.Description,
                            Type = pm.Medicament.Type,
                            Dose = pm.Dose,
                            Details = pm.Details
                        }).ToList()
                }).ToList()
        };

        return patientDto;
    }
}