using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace University
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                return View["index.cshtml"];
            };

            Get["/courses"] = _ => {
                List<Course> AllCourses = Course.GetAll();
                return View["courses.cshtml", AllCourses];
            };

            Get["/courses/new"] = _ => {
                return View["course_new.cshtml"];
            };

            Post["/courses/new"] = _ => {
                Course newCourse = new Course(Request.Form["course-name"], Request.Form["course-number"]);
                newCourse.Save();
                List<Course> AllCourses = Course.GetAll();
                return View["courses.cshtml", AllCourses];
            };

            Get["courses/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Course SelectedCourse = Course.Find(parameters.id);
                List<Student> CourseStudents = SelectedCourse.GetStudents();
                List<Student> AllStudents = Student.GetAll();
                model.Add("course", SelectedCourse);
                model.Add("courseStudents", CourseStudents);
                model.Add("allStudents", AllStudents);
                return View["course.cshtml", model];
            };

            Post["course/add_student"] = _ => {
                Student student = Student.Find(Request.Form["student-id"]);
                Course course = Course.Find(Request.Form["course-id"]);
                course.AddStudent(student);
                return View["success.cshtml"];
            };
// for students
            Get["/students"] = _ => {
                List<Student> AllStudents = Student.GetAll();
                return View["students.cshtml", AllStudents];
            };

            Get["/students/new"] = _ => {
                return View["student_new.cshtml"];
            };

            Post["/students/new"] = _ => {
                Student newStudent = new Student(Request.Form["student-name"], Request.Form["student-date"]);
                newStudent.Save();
                List<Student> AllStudents = Student.GetAll();
                return View["students.cshtml", AllStudents];
            };

            Get["students/{id}"] = parameters => {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Student SelectedStudent = Student.Find(parameters.id);
                List<Course> StudentCourses = SelectedStudent.GetCourses();
                List<Course> AllCourses = Course.GetAll();
                model.Add("student", SelectedStudent);
                model.Add("studentCourses", StudentCourses);
                model.Add("allCourses", AllCourses);
                return View["student.cshtml", model];
            };

            Post["student/add_course"] = _ => {
               Course course = Course.Find(Request.Form["course-id"]);
               Student student = Student.Find(Request.Form["student-id"]);
               student.AddCourse(course);
               return View["success.cshtml"];
           };

// for department
           Get["/departments"] = _ => {
               List<Department> AllDepartments = Department.GetAll();
               return View["departments.cshtml", AllDepartments];
           };

           Get["/departments/new"] = _ => {
               return View["department_new.cshtml"];
           };

           Post["/departments/new"] = _ => {
               Department newDepartment= new Department(Request.Form["department-name"]);
               newDepartment.Save();
               List<Department> AllDepartments = Department.GetAll();
               return View["departments.cshtml", AllDepartments];
           };

           Get["departments/{id}"] = parameters => {
               Dictionary<string, object> model = new Dictionary<string, object>();
               Department SelectedDepartment = Department.Find(parameters.id);
               List<Student> DepartmentStudents = SelectedDepartment.GetStudents();
               List<Student> AllStudents = Student.GetAll();
               List<Course> DepartmentCourses = SelectedDepartment.GetCourses();
               List<Course> AllCourses = Course.GetAll();
               model.Add("department", SelectedDepartment);
               model.Add("departmentStudents", DepartmentStudents);
               model.Add("allStudents", AllStudents);
               model.Add("departmentCourses", DepartmentCourses);
               model.Add("allCourses", AllCourses);
               return View["department.cshtml", model];
           };

           Post["department/add_course"] = _ => {
              Course course = Course.Find(Request.Form["course-id"]);
              Department department = Department.Find(Request.Form["department-id"]);
              department.AddCourse(course);
              return View["success.cshtml"];
          };

          Post["department/add_student"] = _ => {
              Student student = Student.Find(Request.Form["student-id"]);
              Department department = Department.Find(Request.Form["department-id"]);
              department.AddStudent(student);
              return View["success.cshtml"];
          };

        }
    }
}
