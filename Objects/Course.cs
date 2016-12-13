using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace University.Objects
{
  public class Course
  {

    private int _id;
    private string _name;
    private string _courseNumber;

    public Course(string Name, string CourseNumber, int Id = 0)
    {
      _name = Name;
      _courseNumber = CourseNumber;
      _id = Id;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool nameEquality = (this.GetName() == newCourse.GetName());
        bool CourseNumberEquality = (this.GetCourseNumber() == newCourse.GetCourseNumber());
        return (nameEquality && CourseNumberEquality);
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

    public string GetCourseNumber()
    {
      return _courseNumber;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, number) OUTPUT INSERTED.id VALUES (@CourseName, @CourseNumber);", conn);

      SqlParameter studentNameParameter = new SqlParameter("@CourseName", this.GetName());
      cmd.Parameters.Add(studentNameParameter);
      SqlParameter courseNameParameter = new SqlParameter("@CourseNumber", this.GetCourseNumber());
      cmd.Parameters.Add(courseNameParameter);
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

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);
        Course newCourse = new Course(courseName, courseNumber, courseId);
        allCourses.Add(newCourse);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCourses;
    }

    public static Course Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId;", conn);
      SqlParameter courseIdParameter = new SqlParameter("@CourseId", id.ToString());
      cmd.Parameters.Add(courseIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;
      string foundCourseNumber = null;
      while(rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(1);
        foundCourseNumber = rdr.GetString(2);
      }
      Course foundCourse = new Course(foundCourseName, foundCourseNumber, foundCourseId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundCourse;
    }

    public void AddStudent(Student newStudent)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId);", conn);

      SqlParameter courseIdParameter = new SqlParameter("@CourseId", this.GetId());
      cmd.Parameters.Add(courseIdParameter);

      SqlParameter studentIdParameter = new SqlParameter("@StudentId", newStudent.GetId());
      cmd.Parameters.Add(studentIdParameter);

      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
    public List<Student> GetStudents()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN courses_students ON (courses.id = courses_students.course_id) JOIN students ON (courses_students.student_id = students.id) WHERE courses.id=@CourseId;",conn);
      SqlParameter courseIdParameter = new SqlParameter("@CourseId", this.GetId());
      cmd.Parameters.Add(courseIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Student> students = new List<Student>{};
      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime enrollmentDate = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, enrollmentDate, studentId);
        students.Add(newStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return students;
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
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
