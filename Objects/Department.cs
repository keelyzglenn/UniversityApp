using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace University
{
    public class Department
    {
        private int _id;
        private string _name;

        public Department (string Name, int Id = 0)
        {
            _id = Id;
            _name = Name;
        }

        public override bool Equals(System.Object otherDepartment)
        {
            if(!(otherDepartment is Department))
            {
                return false;
            }
            else
            {
                Department newDepartment = (Department) otherDepartment;
                bool idEquality = this.GetId() == newDepartment.GetId();
                bool nameEquality = this.GetName() == newDepartment.GetName();
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string newName)
        {
            _name = newName;
        }


        public static List<Department> GetAll()
        {
            List<Department> AllDepartments = new List<Department> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM departments;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int departmentId = rdr.GetInt32(0);
                string departmentName = rdr.GetString(1);

                Department newDepartment = new Department(departmentName, departmentId);
                AllDepartments.Add(newDepartment);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return AllDepartments;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO departments (name) OUTPUT INSERTED.id VALUES (@DepartmentName)", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@DepartmentName";
            nameParameter.Value = this.GetName();

            cmd.Parameters.Add(nameParameter);

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

        public static Department Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @DepartmentId", conn);

            SqlParameter departmentIdParameter = new SqlParameter();
            departmentIdParameter.ParameterName = "@DepartmentId";
            departmentIdParameter.Value = id.ToString();
            cmd.Parameters.Add(departmentIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundDepartmentId = 0;
            string foundDepartmentName = null;

            while(rdr.Read())
            {
                foundDepartmentId = rdr.GetInt32(0);
                foundDepartmentName = rdr.GetString(1);
            }
            Department foundDepartment= new Department(foundDepartmentName, foundDepartmentId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundDepartment;
        }

        public void AddStudent(Student newStudent)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO courses_students (department_id, student_id) VALUES (@DepartmentId, @StudentId)", conn);

            SqlParameter departmentIdParameter = new SqlParameter();
            departmentIdParameter.ParameterName = "@DepartmentId";
            departmentIdParameter.Value = this.GetId();
            cmd.Parameters.Add(departmentIdParameter);

            SqlParameter studentIdParameter = new SqlParameter();
            studentIdParameter.ParameterName = "@StudentId";
            studentIdParameter.Value = newStudent.GetId();
            cmd.Parameters.Add(studentIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }


        public List<Student> GetStudents()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT students.* FROM departments JOIN courses_students ON (departments.id = courses_students.department_id) JOIN students ON (courses_students.student_id = students.id) WHERE departments.id = @DepartmentId;", conn);
            SqlParameter departmentIdParameter = new SqlParameter();
            departmentIdParameter.ParameterName = "@DepartmentId";
            departmentIdParameter.Value = this.GetId().ToString();

            cmd.Parameters.Add(departmentIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Student> students = new List<Student> {};

            while(rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                string studentDate = rdr.GetString(2);

                Student newStudent = new Student(studentName, studentDate, studentId);
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

        public void AddCourse(Course newCourse)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO courses_students (department_id, course_id) VALUES (@DepartmentId, @CourseId)", conn);

            SqlParameter courseIdParameter = new SqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = newCourse.GetId();
            cmd.Parameters.Add(courseIdParameter);

            SqlParameter departmentIdParameter = new SqlParameter();
            departmentIdParameter.ParameterName = "@DepartmentId";
            departmentIdParameter.Value = this.GetId();
            cmd.Parameters.Add(departmentIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Course> GetCourses()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT courses.* FROM departments JOIN courses_students ON (departments.id = courses_students.department_id) JOIN courses ON (courses_students.course_id = courses.id) WHERE departments.id = @DepartmentId;", conn);
            SqlParameter departmentIdParameter = new SqlParameter();
            departmentIdParameter.ParameterName = "@DepartmentId";
            departmentIdParameter.Value = this.GetId().ToString();

            cmd.Parameters.Add(departmentIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Course> courses = new List<Course> {};

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



        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
