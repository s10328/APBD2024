using GakkoAppHorizontalTests.TestDoubles;
using GakkoHorizontalSlice.Services;

namespace GakkoAppHorizontalTests;

public class StudentsServiceTests
{
    [Fact]
    public void GetStudents_Should_Return_List_Of_Students()
    {
        //Arrange
        var studentsService = new StudentsService(new TestStudentsRepository());
        
        //Act
        var result = studentsService.GetStudents();

        //Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("John", result.First().FirstName);
        Assert.Equal("Doe", result.First().LastName);
        Assert.Equal(1, result.First().IdStudent);
    }
}