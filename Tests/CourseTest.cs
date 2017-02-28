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

        public void Dispose()
        {
            Course.DeleteAll();
            Student.DeleteAll();
        }
    }
}
