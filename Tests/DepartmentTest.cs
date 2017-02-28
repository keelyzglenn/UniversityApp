using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace University
{
    public class DepartmentTest : IDisposable
    {
        public DepartmentTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_EqualOverride_True()
        {
            Department firstDepartment = new Department("History");
            Department secondDepartment = new Department("History");

            Assert.Equal(firstDepartment, secondDepartment);
        }

        [Fact]
        public void Test_Save()
        {
            //Arrange
            Department firstDepartment = new Department("History");
            firstDepartment.Save();

            //Act
            List<Department> result = Department.GetAll();
            List<Department> testList = new List<Department>{firstDepartment};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_SaveAssignsIdToObject()
        {
            //Arrange
            Department firstDepartment = new Department("History");
            firstDepartment.Save();

            //Act
            Department savedDepartment = Department.GetAll()[0];

            int result = savedDepartment.GetId();
            int testId = firstDepartment.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_FindFindsDepartmentInDatabase()
        {
            //Arrange
            Department firstDepartment = new Department("History");
            firstDepartment.Save();

            //Act
            Department result = Department.Find(firstDepartment.GetId());

            //Assert
            Assert.Equal(firstDepartment, result);
        }

        [Fact]
        public void Test_AddStudent_AddsStudentToDepartment()
        {
            //Arrange
            Department testDepartment = new Department("History");
            testDepartment.Save();

            Student testStudent = new Student("Jerry", "03/01/2012");
            testStudent.Save();

            //Act
            testDepartment.AddStudent(testStudent);

            List<Student> result = testDepartment.GetStudents();
            List<Student> testList = new List<Student>{testStudent};

            //Assert
            Assert.Equal(testList, result);
        }

        [Fact]
        public void Test_AddCourse_AddsCourseToDepartment()
        {
            //Arrange
            Department testDepartment = new Department("History");
            testDepartment.Save();

            Course testCourse = new Course("History", "100");
            testCourse.Save();

            //Act
            testDepartment.AddCourse(testCourse);

            List<Course> result = testDepartment.GetCourses();
            List<Course> testList = new List<Course>{testCourse};

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
