namespace WebApplication2.Data;

//KROK 2
//Skoro mamy już zdefiniowane wszystkie modele, następnym logicznym krokiem jest stworzenie kontekstu bazy danych (DbContext).
//Jest to kluczowy element, który łączy nasze modele z faktyczną bazą danych.
// Stwórzmy nowy folder o nazwie "Data" w naszym projekcie i dodajmy w nim nową klasę PrescriptionDbContext:

using Microsoft.EntityFrameworkCore;
using WebApplication2.Models; //importujemy nasze modele

public class PrescriptionDbContext : DbContext
{
    //tworzymy konstruktor przyjmujacy opcje konfiguracji
    public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options)
        : base(options) //brakujaca czesc konstruktora
    {
        
    }
    
    //Definicja DbSetow dla kazdej z naszych encji
    //DBSet reprezentuje tabele w bazie danych
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Prescription_Medicament> Prescriptions_Medicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //konfiguruje klucz zlozony dla tabeli laczacej (PrescriptionMedicament)
        modelBuilder.Entity<Prescription_Medicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription }); //pm zmienna tymczasowa - od Prescription_Medicament,
        //pm.IdMedicament i pm.IdPrescription to pola ktore razem tworza unikalny identyfikator
        
    }

}