using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace University
{
    public class Student
    {
        private int _id;
        private string _name;
        private string _date;

        public Student(string Name, string Date, int Id = 0)
        {
            _id = Id;
            _name = Name;
            _date = Date;
        }

        public override bool Equals(System.Object otherStudent)
        {
            if(!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student) otherStudent;
                bool idEquality = this.GetId() == newStudent.GetId();
                bool nameEquality = this.GetName() == newStudent.GetName();
                bool dateEquality = this.GetDate() == newStudent.GetDate();
                return (idEquality && nameEquality && dateEquality);
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

        public string GetDate()
        {
            return _date;
        }

        public void SetDate(string newDate)
        {
            _date = newDate;
        }

        public static List<Student> GetAll()
        {
            List<Student> AllStudents = new List<Student> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                string studentDate = rdr.GetString(2);

                Student newStudent = new Student(studentName, studentDate, studentId);
                AllStudents.Add(newStudent);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return AllStudents;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO students (name, date) OUTPUT INSERTED.id VALUES (@StudentName, @StudentDate)", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@StudentName";
            nameParameter.Value = this.GetName();

            SqlParameter dateParameter = new SqlParameter();
            dateParameter.ParameterName = "@StudentDate";
            dateParameter.Value = this.GetDate();

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(dateParameter);

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

        public static Student Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId", conn);

            SqlParameter studentIdParameter = new SqlParameter();
            studentIdParameter.ParameterName = "@StudentId";
            studentIdParameter.Value = id.ToString();
            cmd.Parameters.Add(studentIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundStudentId = 0;
            string foundStudentName = null;
            string foundStudentDate = null;

            while(rdr.Read())
            {
                foundStudentId = rdr.GetInt32(0);
                foundStudentName = rdr.GetString(1);
                foundStudentDate = rdr.GetString(2);
            }
            Student foundStudent = new Student(foundStudentName, foundStudentDate, foundStudentId);

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
