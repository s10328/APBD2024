namespace GakkoAppVertical.Students;

public static class Configuration
{
    
    public static IEndpointRouteBuilder RegisterStudentsUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/students", (IStudentsService service) => TypedResults.Ok(service.GetStudents()));
        endpoints.MapGet("/students/{id:int}", (int id, IStudentsService service) => TypedResults.Ok(service.GetStudent(id)));
        endpoints.MapPost("/students", (Student student, IStudentsService service) => TypedResults.Created("", service.CreateStudent(student)));
        endpoints.MapPut("/students/{id:int}", (int id, Student student, IStudentsService service) =>
        {
            student.IdStudent = id;
            service.UpdateStudent(student);
            return TypedResults.NoContent();
        });
        endpoints.MapDelete("/students/{id:int}", (int id, IStudentsService service) =>
        {
            service.DeleteStudent(id);
            return TypedResults.NoContent();
        });
        
        return endpoints;
    }
    
}