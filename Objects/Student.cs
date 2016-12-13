using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace University.Objects
{
  public class Student
  {

    private int _id;
    private string _name;
    private DateTime _enrollmentDate = new DateTime();

    public Student(string Name, DateTime EnrollmentDate, int Id = 0)
    {
      _name = Name;
      _enrollmentDate = EnrollmentDate;
      _id = Id;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool nameEquality = (this.GetName() == newStudent.GetName());
        return (nameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public DateTime GetEnrollmentDate()
    {
      return _enrollmentDate;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (name, enrollment_date) OUTPUT INSERTED.id VALUES (@StudentName, @EnrollmentDate);", conn);

      SqlParameter studentNameParameter = new SqlParameter("@StudentName", this.GetName());
      cmd.Parameters.Add(studentNameParameter);
      SqlParameter enrollmentParameter = new SqlParameter("@EnrollmentDate", this.GetEnrollmentDate());
      cmd.Parameters.Add(enrollmentParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime enrollmentDate = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, enrollmentDate, studentId);
        allStudents.Add(newStudent);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allStudents;
    }

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter("@StudentId", id.ToString());
      cmd.Parameters.Add(studentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      DateTime foundStudentEnrollment = new DateTime(16, 12, 13);
      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
        foundStudentEnrollment = rdr.GetDateTime(2);
      }
      Student foundStudent = new Student(foundStudentName, foundStudentEnrollment, foundStudentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundStudent;
    }

    public void AddCourse(Course newCourse)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INT0 courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);", conn);

      SqlParameter courseIdParameter = new SqlParameter("@CourseId", newCourse.GetId());
      cmd.Parameters.Add(courseIdParameter);

      SqlParameter studentIdParameter = new SqlParameter("@StudentId", this.GetId());
      cmd.Parameters.Add(studentIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
    public List<Course> GetCourses()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN courses_students ON (students.id = courses_students.student_id) JOIN courses ON (courses_students.course_id = courses.id) WHERE students.id=@StudentId;",conn);
      SqlParameter studentIdParameter = new SqlParameter("@StudentId", this.GetId());
      cmd.Parameters.Add(studentIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Course> courses = new List<Course>{};
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);
        Course newCourse = new Course(courseName, courseNumber, courseId);
        courses.Add(newCourse);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return courses;
    }


    // public void Edit(string description)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //   Console.WriteLine(this.TEMPLATEdescription);
    //
    //   SqlCommand cmd = new SqlCommand("UPDATE template SET TEMPLATEdescription = @TEMPLATEdescription WHERE id = @TEMPLATEId;", conn);
    //
    //   SqlParameter TEMPLATEParameter = new SqlParameter("@TEMPLATEId", this.Id);
    //
    //    SqlParameter TEMPLATEdescriptionParameter = new SqlParameter("TEMPLATEdescription", description);
    //
    //    cmd.Parameters.Add(TEMPLATEParameter);
    //    cmd.Parameters.Add(TEMPLATEdescriptionParameter);
    //
    //    SqlDataReader rdr = cmd.ExecuteReader();
    //
    //    while(rdr.Read())
    //    {
    //      this.Id = rdr.GetInt32(0);
    //      this.TEMPLATEdescription = rdr.GetString(1);
    //    }
    //    if (rdr != null)
    //    {
    //      rdr.Close();
    //    }
    //    if (conn != null)
    //    {
    //      conn.Close();
    //    }
    //  }
    //
    // public static List<TEMPLATE> Sort()
    // {
    //   List<Task> allTEMPLATE = new List<Task>{};
    //
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT * FROM template ORDER BY TEMPLATEdate;", conn);
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   while(rdr.Read())
    //   {
    //     int TEMPLATEId = rdr.GetInt32(0);
    //     string TEMPLATEDescription = rdr.GetString(1);
    //     TEMPLATE newTEMPLATE = new TEMPLATE(TEMPLATEDescription, TEMPLATEId);
    //     allTEMPLATE.Add(newTEMPLATE);
    //   }
    //
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    //
    //   return allTEMPLATE;
    // }
    // public void Delete()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //   SqlCommand cmd = new SqlCommand("DELETE FROM template WHERE id = @TEMPLATEId; DELETE FROM join_table WHERE template_id = @TEMPLATEId", conn);
    //   SqlParameter TEMPLATEIdParameter = new SqlParameter("@TEMPLATEId", this.Id);
    //   cmd.Parameters.Add(TEMPLATEIdParameter);
    //   cmd.ExecuteNonQuery();
    //
    //   if(conn!=null)
    //   {
    //     conn.Close();
    //   }
    // }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
