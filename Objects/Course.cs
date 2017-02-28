using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace University
{


    public class Course
    {
        private int _id;
        private string _name;
        private string _number;

        public Course (string Name, string Number, int Id = 0)
        {
            _id = Id;
            _name = Name;
            _number = Number;
        }

        public override bool Equals(System.Object otherCourse)
        {
            if(!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course) otherCourse;
                bool idEquality = this.GetId() == newCourse.GetId();
                bool nameEquality = this.GetName() == newCourse.GetName();
                bool numberEquality = this.GetNumber() == newCourse.GetNumber();
                return (idEquality && nameEquality && numberEquality);
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

        public string GetNumber()
        {
            return _number;
        }

        public void SetNumber(string newNumber)
        {
            _number = newNumber;
        }

        public static List<Course> GetAll()
        {
            List<Course> AllCourses = new List<Course> {};

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
                AllCourses.Add(newCourse);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return AllCourses;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO courses (name, number) OUTPUT INSERTED.id VALUES (@CourseName, @CourseNumber)", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@CourseName";
            nameParameter.Value = this.GetName();

            SqlParameter numberParameter = new SqlParameter();
            numberParameter.ParameterName = "@CourseNumber";
            numberParameter.Value = this.GetNumber();

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(numberParameter);

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

        public static Course Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId", conn);

            SqlParameter courseIdParameter = new SqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = id.ToString();
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

            SqlCommand cmd = new SqlCommand("INSERT INTO courses_students (course_id, student_id) VALUES (@CourseId, @StudentId)", conn);

            SqlParameter courseIdParameter = new SqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = this.GetId();
            cmd.Parameters.Add(courseIdParameter);

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

            SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN courses_students ON (courses.id = courses_students.course_id) JOIN students ON (courses_students.student_id = students.id) WHERE courses.id = @CourseId;", conn);
            SqlParameter courseIdParameter = new SqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = this.GetId().ToString();

            cmd.Parameters.Add(courseIdParameter);

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
