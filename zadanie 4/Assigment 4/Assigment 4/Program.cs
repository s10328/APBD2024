using Assigment_4.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var animals = new List<Animal>
{
    new Animal { Id = 1, Name = "Roma", Category = "Pies", Weight = 7.0, FurColor = "Czarny"},
    new Animal { Id = 2, Name = "Major", Category = "Pies", Weight = 1.5, FurColor = "Siwy"},
    new Animal { Id = 3, Name = "Puszek", Category = "Kot", Weight = 4.0, FurColor = "Czarny"}
};
var visits = new List<Visit>
{
    new Visit {Id = 001, AnimalId = 1, DateOfVisit = new DateTime(2023,07,15), Description = "Odrobaczanie", Price = 100},
    new Visit {Id = 002, AnimalId = 2, DateOfVisit = new DateTime(2023,08,25), Description = "Zabieg", Price = 300},
    new Visit {Id = 003, AnimalId = 1, DateOfVisit = new DateTime(2023,06,05), Description = "Sterylizacja", Price = 500},
};

app.MapGet("/animals", () => animals);

app.MapGet("/animals/{id}", (int id) => animals.FirstOrDefault(a => a.Id == id));

app.MapPost("/animals", (Animal animal) => {
    animal.Id = animals.Any() ? animals.Max(a => a.Id) + 1 : 1;
    animals.Add(animal);
    return Results.Created($"/animals/{animal.Id}", animal);
});

app.MapPut("/animals/{id}", (int id, Animal updatedAnimal) => {
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal == null) return Results.NotFound();
    animal.Name = updatedAnimal.Name;
    animal.Category = updatedAnimal.Category;
    animal.Weight = updatedAnimal.Weight;
    animal.FurColor = updatedAnimal.FurColor;
    return Results.NoContent();
});

app.MapDelete("/animals/{id}", (int id) => {
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal == null) return Results.NotFound();
    animals.Remove(animal);
    return Results.NoContent();
});

app.MapGet("/animals/{animalId}/visits", (int animalId) => visits.Where(v => v.AnimalId == animalId));

app.MapPost("/visits", (Visit visit) => {
    visit.Id = visits.Any() ? visits.Max(v => v.Id) + 1 : 1;
    visits.Add(visit);
    return Results.Created($"/visits/{visit.Id}", visit);
});

app.Run();
