using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace University.Objects
{
  public class StudentTest : IDisposable
  {
    DateTime newDateTime = new DateTime(2016, 12, 13);
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void ReplacesEqualObjects_True()
    {

      Student studentOne = new Student("Daniel", newDateTime);
      Student studentTwo = new Student("Daniel", newDateTime);

      Assert.Equal(studentOne, studentTwo);
    }
    [Fact]
    public void GetAll_true()
    {
      //Arrange
      Student studentOne = new Student("Daniel", newDateTime);
      studentOne.Save();
      Student studentTwo = new Student("Ryan", newDateTime);
      studentTwo.Save();
      // Act
      int result = Student.GetAll().Count;

      //Assert
      Assert.Equal(2, result);
    }

    [Fact]
    public void Save_SavesToDatabase_true()
    {
      //Arrange
      Student testStudent = new Student("Jimmy", newDateTime);
      testStudent.Save();
      //Act

      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Find_FindsStudentInDatabase_true()
    {
      //Arrange
      Student testStudent = new Student("Ryan", newDateTime);
      testStudent.Save();

      //Act
      Student foundStudent = Student.Find(testStudent.GetId());

      //Assert
      Assert.Equal(testStudent, foundStudent);
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
    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();
    }

  }

}
