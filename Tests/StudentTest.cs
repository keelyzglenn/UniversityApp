using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace University
{
    public class StudentTest : IDisposable
    {
        public StudentTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EqualOverride_True()
        {
            Student firstStudent = new Student("Jerry", "03/01/2012");
            Student secondStudent = new Student("Jerry", "03/01/2012");

            Assert.Equal(firstStudent, secondStudent);
        }

        [Fact]
        public void Test_Save()
        {
            //Arrange
            Student firstStudent = new Student("Jerry", "03/01/2012");
            firstStudent.Save();

            //Act
            List<Student> result = Student.GetAll();
            List<Student> testList = new List<Student>{firstStudent};

            //Assert
            Assert.Equal(testList, result);
        }


        public void Dispose()
        {
            Student.DeleteAll();
            Course.DeleteAll();
        }
    }
}
