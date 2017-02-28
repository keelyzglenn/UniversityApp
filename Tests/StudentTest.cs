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


        [Fact]
        public void Test_SaveAssignsIdToObject()
        {
            //Arrange
            Student firstStudent = new Student("Jerry", "03/01/2012");
            firstStudent.Save();

            //Act
            Student savedStudent = Student.GetAll()[0];

            int result = savedStudent.GetId();
            int testId = firstStudent.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_FindFindsStudentInDatabase()
        {
            //Arrange
            Student firstStudent = new Student("Jerry", "03/01/2012");
            firstStudent.Save();

            //Act
            Student result = Student.Find(firstStudent.GetId());

            //Assert
            Assert.Equal(firstStudent, result);
        }


        public void Dispose()
        {
            Student.DeleteAll();
            Course.DeleteAll();
        }
    }
}
