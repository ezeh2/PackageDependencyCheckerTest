using CyberSecurity3WebApplication.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSecurity3WebApplication.Controllers
{
    public class SchoolController : Controller
    {
        private SchoolContext schoolContext;
        private IConfiguration configuration;

        public SchoolController(IConfiguration configuration, SchoolContext schoolContext)
        {
            this.configuration = configuration;
            this.schoolContext = schoolContext;
        }

        /// <summary>
        /// https://localhost:44365/school/Example100_FindByStudentId?studentId=1
        /// 
        /// dump full School-table
        /// https://localhost:44365/school/Example100_FindByStudentId?studentId=1%20%20or%201=1%20--
        /// </summary>
        /// <returns></returns>
        public IActionResult Example100_FindByStudentId(string studentId)
        {
            return Content(FindStudent($"id={studentId}"));
        }

        /// <summary>
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=Alexander
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=Alexander%27%20or%201=1%20--
        /// 
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=bla%27%20union%20select%201,null,null,null%20from%20sqlite_schema%20--
        /// 
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=bla%27%20union%20select%20rootpage,type,name,sql%20from%20sqlite_schema%20--
        /// 
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=bla%27%20union%20select%20EnrollmentID,CourseID,StudentID,Grade%20from%20Enrollment%20--
        /// </summary>
        /// <param name="studentLastName"></param>
        /// <returns></returns>
        public IActionResult Example100_FindByStudentLastName(string studentLastName)
        {
            return Content(FindStudent($"LastName='{studentLastName}'"));
        }

        /// <summary>
        /// https://localhost:44365/school/Example100_FindByStudentFirstMidName?studentFirstMidName=Carson
        /// https://localhost:44365/school/Example100_FindByStudentFirstMidName?studentFirstMidName=Carson%27%20or%201=1%20--
        /// </summary>
        /// <param name="studentFirstMidName"></param>
        /// <returns></returns>
        public IActionResult Example100_FindByStudentFirstMidName(string studentFirstMidName)
        {
            return Content(FindStudent($"FirstMidName='{studentFirstMidName}'"));
        }

        private string FindStudent(string where)
        {
            SqliteConnection conn = new SqliteConnection(configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"select ID,LastName,FirstMidName,EnrollmentDate from Student where {where}";
            SqliteDataReader reader = cmd.ExecuteReader();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CommandText: ");
            sb.AppendLine(cmd.CommandText);
            sb.AppendLine();

            while (reader.Read())
            {
                sb.Append("ID: ").Append(reader[0]).AppendLine();
                sb.Append("LastName: ").Append(reader[1]).AppendLine();
                sb.Append("FirstMidName: ").Append(reader[2]).AppendLine();
                sb.Append("EnrollmentDate: ").Append(reader[3]).AppendLine();
            }
            return sb.ToString();
        }

        public IActionResult Example110(string studentLastName)
        {
            // var students = schoolContext.Students.ToList();
            // schoolContext.Students.Add(new Student { LastName = "x", FirstMidName = "y" });
            // int cnt = schoolContext.SaveChanges();

            return Content("");
        }
    }
}
