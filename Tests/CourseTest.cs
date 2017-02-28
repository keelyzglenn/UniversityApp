using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace University
{
    public class CourseTest : IDisposable
    {
        public CourseTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EqualOverride_True()
        {
            Course firstCourse = new Course("History", "100");
            Course secondCourse = new Course("History", "100");

            Assert.Equal(firstCourse, secondCourse);
        }

        [Fact]
        public void Test_Save()
        {
            //Arrange
            Course firstCourse = new Course("History", "100");
            firstCourse.Save();

            //Act
            List<Course> result = Course.GetAll();
            List<Course> testList = new List<Course>{firstCourse};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_SaveAssignsIdToObject()
        {
            //Arrange
            Course firstCourse = new Course("History", "100");
            firstCourse.Save();

            //Act
            Course savedCourse = Course.GetAll()[0];

            int result = savedCourse.GetId();
            int testId = firstCourse.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_FindFindsCourseInDatabase()
        {
            //Arrange
            Course firstCourse = new Course("History", "100");
            firstCourse.Save();

            //Act
            Course result = Course.Find(firstCourse.GetId());

            //Assert
            Assert.Equal(firstCourse, result);
        }

        [Fact]
        public void Test_AddStudent_AddsStudentToCourse()
        {
            //Arrange
            Course testCourse = new Course("History", "100");
            testCourse.Save();

            Student testStudent = new Student("Jerry", "03/01/2012");
            testStudent.Save();

            //Act
            testCourse.AddStudent(testStudent);

            List<Student> result = testCourse.GetStudents();
            List<Student> testList = new List<Student>{testStudent};

            //Assert
            Assert.Equal(testList, result);
        }

        public void Dispose()
        {
            Course.DeleteAll();
            Student.DeleteAll();
            Department.DeleteAll();
        }
    }
}
