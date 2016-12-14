using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace University.Objects
{
  public class CourseTest : IDisposable
  {
    DateTime newDateTime = new DateTime(2016, 12, 13);
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

    [Fact]
    public void AddStudent_AddsStudentToCourse_True()
    {
      Student newStudent = new Student("Ryan", newDateTime);
      newStudent.Save();
      Course newCourse = new Course("Ryan", "Ryan101");
      newCourse.Save();
      newCourse.AddStudent(newStudent);
      List<Student> expected = new List<Student>{newStudent};
      List<Student> result = newCourse.GetStudents();

      Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Deletes_Course()
    {
      Course newCourse = new Course("Ryan", "ryan101");

      newCourse.Save();
      newCourse.Delete();

      List<Course> expected = new List<Course>{};
      List<Course> result = Course.GetAll();

      Assert.Equal(expected, result);

    }
    [Fact]
    public void Delete_DeletesAssociation_True()
    {
      Student newStudent = new Student("Ryan", newDateTime);
      newStudent.Save();
      Course newCourse = new Course("Ryan", "Ryan101");
      newCourse.Save();
      newCourse.AddStudent(newStudent);
      newCourse.Delete();

      List<Student> result = newCourse.GetStudents();
      List<Student> expected = new List<Student>{};
      Assert.Equal(expected, result);

    }
    [Fact]
    public void Update_UpdatesDatabase_True()
    {
      Course newCourse = new Course("Ryan", "Ryan101");
      newCourse.Save();
      newCourse.Update("Daniel", "Daniel335");

      Course testCourse = new Course("Daniel", "Daniel335");
      Course result = Course.Find(newCourse.GetId());

      Assert.Equal(testCourse, result);
    }

    public void Dispose()
    {
      Course.DeleteAll();
    }

  }
}
