using GakkoHorizontalSlice.Model;
using GakkoHorizontalSlice.Repositories;

namespace GakkoAppHorizontalTests.TestDoubles;

public class TestStudentsRepository : IStudentsRepository
{
    public IEnumerable<Student> GetStudents()
    {
        return new List<Student>
        {
            new Student
            {
                IdStudent = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "Warsaw, Zlota 12",
                Email = "doe@wp.pl",
                IndexNumber = 1
            }
        };
    }

    public int CreateStudent(Student student)
    {
        throw new NotImplementedException();
    }

    public Student GetStudent(int idStudent)
    {
        throw new NotImplementedException();
    }

    public int UpdateStudent(Student student)
    {
        throw new NotImplementedException();
    }

    public int DeleteStudent(int idStudent)
    {
        throw new NotImplementedException();
    }
}