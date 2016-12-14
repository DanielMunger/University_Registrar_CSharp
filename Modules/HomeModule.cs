using Nancy;
using System.Collections.Generic;
using System;
using University.Objects;

namespace University
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/courses"] = _ =>{
        List<Course> allCourses = Course.GetAll();
        return View["courses.cshtml", allCourses];
      };
      Get["/students"] = _ =>{
        List<Student> allStudents = Student.GetAll();
        return View["students.cshtml", allStudents];
      };
      Get["/course/new"] = _ => {
        return View["new_course.cshtml"];
      };
      Post["/course/new"] = _ => {
        Course newCourse = new Course(Request.Form["course-name"] , Request.Form["course-number"]);
        newCourse.Save();
        return View["new_course.cshtml", newCourse];
      };
      Get["/student/new"] = _ => {
        return View["new_student.cshtml"];
      };
      Post["/student/new"] = _ => {
        var date = Request.Form["student-enrollment"];
        Student newStudent = new Student(Request.Form["student-name"] , date);
        newStudent.Save();
        return View["new_student.cshtml", newStudent];
      };
      Get["/course/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course foundCourse = Course.Find(parameters.id);
        List<Student> students = foundCourse.GetStudents();
        List<Student> allStudents = Student.GetAll();
        model.Add("current-course", foundCourse);
        model.Add("students-in-course", students);
        model.Add("all-students", allStudents);
        return View["course.cshtml", model];
      };
      Post["/course/add_student"] = _ =>{
        Course course = Course.Find(Request.Form["course-id"]);
        Student student = Student.Find(Request.Form["student-id"]);
        course.AddStudent(student);
        return View["success.cshtml"];
      };
      Get["/student/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student foundStudent = Student.Find(parameters.id);
        List<Course> courses = foundStudent.GetCourses();
        List<Course> allCourses = Course.GetAll();
        model.Add("current-student", foundStudent);
        model.Add("courses-enrolled", courses);
        model.Add("all-courses", allCourses);
        return View["student.cshtml", model];
      };
      Post["/student/add_course"] = _ => {
        Student newStudent = Student.Find(Request.Form["student-id"]);
        Course newCourse = Course.Find(Request.Form["course-id"]);
        newStudent.AddCourse(newCourse);
        return View["success.cshtml"];
      };
      Get["/student/delete/{id}"] = parameters => {
        Student newStudent = Student.Find(parameters.id);
        return View["student_delete.cshtml", newStudent];
      };
      Delete["/student/delete/{id}"] = parameters => {
        Student selectedStudent = Student.Find(parameters.id);
        selectedStudent.Delete();
        return View["success.cshtml"];
      };
      Get["/course/delete/{id}"] = parameters => {
        Course newCourse = Course.Find(parameters.id);
        return View["course_delete.cshtml", newCourse];
      };
      Delete["/course/delete/{id}"] = parameters => {
        Course selectedCourse = Course.Find(parameters.id);
        selectedCourse.Delete();
        return View["success.cshtml"];
      };
    }
  }
}
