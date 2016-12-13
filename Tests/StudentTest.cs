using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace University.Objects
{
  public class CourseTest : IDisposable
  {
    DateTime newDateTime = new DateTime(12,3,4);
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void ReplacesEqualObjects_True()
    {

      Course CourseOne = new Course("Daniel", "DAN101");
      Course CourseTwo = new Course("Daniel",  "DAN101");

      Assert.Equal(CourseOne, CourseTwo);
    }
    [Fact]
    public void GetAll_true()
    {
      //Arrange
      Course CourseOne = new Course("Daniel",  "DAN101");
      CourseOne.Save();
      Course CourseTwo = new Course("Ryan",  "RYAN101");
      CourseTwo.Save();
      // Act
      int result = Course.GetAll().Count;

      //Assert
      Assert.Equal(2, result);
    }

    [Fact]
    public void Save_SavesToDatabase_true()
    {
      //Arrange
      Course testCourse = new Course("Jimmy", "Jim101");
      testCourse.Save();
      //Act

      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Find_FindsCourseInDatabase_true()
    {
      //Arrange
      Course testCourse = new Course("Ryan", "RYAN101");
      testCourse.Save();

      //Act
      Course foundCourse = Course.Find(testCourse.GetId());

      //Assert
      Assert.Equal(testCourse, foundCourse);
    }
    public void AddCourse_AddsCourseToStudent_True()
    {
      Course newCourse = new Course("Ryan", "ryan101");
      newCourse.Save();
      Student newStudent = new Student("Ryan", newDateTime);
      newStudent.Save();
      newStudent.AddCourse(newCourse);
      List<Course> expected = new List<Course>{newCourse};
      List<Course> result = newStudent.GetCourses();

      Assert.Equal(expected, result);
    }

    public void Dispose()
    {
      Course.DeleteAll();
    }

  }
}
