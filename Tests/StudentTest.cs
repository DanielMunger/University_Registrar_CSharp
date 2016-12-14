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

    [Fact]
    public void Test_Deletes_Student()
    {
      Student newStudent = new Student("Ryan", newDateTime);

      newStudent.Save();
      newStudent.Delete();

      List<Student> expected = new List<Student>{};
      List<Student> result = Student.GetAll();

      Assert.Equal(expected, result);

    }

    [Fact]
    public void Update_UpdatesDatabase_True()
    {
      Student newStudent = new Student("Ryan", newDateTime);
      newStudent.Save();
      newStudent.Update("Daniel", newDateTime);

      Student testStudent = new Student("Daniel", newDateTime);
      Student result = Student.Find(newStudent.GetId());

      Assert.Equal(testStudent, result);
    }

    [Fact]
    public void Delete_DeletesAssociation_True()
    {
      Student newStudent = new Student("Ryan", newDateTime);
      newStudent.Save();
      Course newCourse = new Course("Ryan", "Ryan101");
      newCourse.Save();
      newStudent.AddCourse(newCourse);
      newStudent.Delete();

      List<Course> result = newStudent.GetCourses();
      List<Course> expected = new List<Course>{};
      Assert.Equal(expected, result);

    }

    public void Dispose()
    {
      Course.DeleteAll();
      Student.DeleteAll();
    }

  }

}
