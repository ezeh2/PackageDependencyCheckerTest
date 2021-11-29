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
            return Content(FindStudent(studentId, null, null));
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
            return Content(FindStudent(null, studentLastName,null));
        }

        /// <summary>
        /// https://localhost:44365/school/Example100_FindByStudentFirstMidName?studentFirstMidName=Carson
        /// https://localhost:44365/school/Example100_FindByStudentFirstMidName?studentFirstMidName=Carson%27%20or%201=1%20--
        /// </summary>
        /// <param name="studentFirstMidName"></param>
        /// <returns></returns>
        public IActionResult Example100_FindByStudentFirstMidName(string studentFirstMidName)
        {
            return Content(FindStudent(null,null, studentFirstMidName));
        }

        private string FindStudent(string studentId, string studentLastName, string studentFirstMidName)
        {
            return FindStudent2(studentId, studentLastName, studentFirstMidName);
        }

        private string FindStudent1(string studentId, string studentLastName, string studentFirstMidName)
        {
            SqliteConnection conn = new SqliteConnection(configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            string where = null;
            if (studentId != null)
            {
                where = $"where id={studentId}";
            }
            else if (studentLastName != null)
            {
                where = $"where LastName='{studentLastName}'";
            }
            else if (studentFirstMidName != null)
            {
                where = $"where FirstMidName='{studentFirstMidName}'";
            }

            SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"select ID,LastName,FirstMidName,EnrollmentDate from Student {where}";

            return StudentToString(cmd);
        }

        private string FindStudent2(string studentId, string studentLastName, string studentFirstMidName)
        {
            using (SqliteConnection conn = new SqliteConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                StringBuilder sb = new StringBuilder();
                sb.Append("select ID,LastName,FirstMidName,EnrollmentDate from Student where ");

                SqliteCommand cmd = conn.CreateCommand();
                if (studentId != null)
                {
                    sb.Append("id = @ID");
                    cmd.Parameters.Add("ID", SqliteType.Integer);
                    cmd.Parameters["ID"].Value = studentId;
                }
                if (studentLastName != null)
                {
                    sb.Append("LastName = @LastName");
                    cmd.Parameters.Add("LastName", SqliteType.Text);
                    cmd.Parameters["LastName"].Value = studentLastName;
                }
                if (studentFirstMidName != null)
                {
                    sb.Append("FirstMidName = @FirstMidName");
                    cmd.Parameters.Add("FirstMidName", SqliteType.Text);
                    cmd.Parameters["FirstMidName"].Value = studentFirstMidName;
                }
                cmd.CommandText = sb.ToString();

                return StudentToString(cmd);
            }
        }

        private string StudentToString(SqliteCommand cmd)
        {
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
