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
    /// <summary>
    /// demos "SQL injection"-vulnerabilities
    /// </summary>
    public class SchoolController : Controller
    {
        private IConfiguration configuration;

        public SchoolController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// normal use
        /// https://localhost:44365/school/Example100_FindByStudentId?studentId=1
        /// 
        /// abuse SQL-injection: dump full table "Students"
        /// https://localhost:44365/school/Example100_FindByStudentId?studentId=1%20%20or%201=1%20--
        /// </summary>
        /// <returns></returns>
        public IActionResult Example100_FindByStudentId(string studentId)
        {
            return Content(FindStudent_V1(studentId, null, null));
        }

        /// <summary>
        /// normal use
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=Alexander
        /// 
        /// abuse SQL-injection: dump full School-table
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=Alexander%27%20or%201=1%20--
        /// 
        /// abuse SQL-injection: dump all tables-names including column-names
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=bla%27%20union%20select%20rootpage,type,name,sql%20from%20sqlite_schema%20--
        /// 
        /// abuse SQL-injection: dump full table "Enrollment" 
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=bla%27%20union%20select%20EnrollmentID,CourseID,StudentID,Grade%20from%20Enrollment%20--
        /// 
        /// abuse SQL-injection: dump full table "Course" 
        /// https://localhost:44365/school/Example100_FindByStudentLastName?studentLastName=bla%27%20union%20select%20CourseID,Title,Null,Null%20from%20Course%20--
        /// </summary>
        /// <param name="studentLastName"></param>
        /// <returns></returns>
        public IActionResult Example100_FindByStudentLastName(string studentLastName)
        {
            return Content(FindStudent_V1(null, studentLastName,null));
        }

        /// <summary>
        /// normal use
        /// https://localhost:44365/school/Example100_FindByStudentFirstMidName?studentFirstMidName=Carson
        /// 
        /// abuse SQL-injection: dump full table "Students"
        /// https://localhost:44365/school/Example100_FindByStudentFirstMidName?studentFirstMidName=Carson%27%20or%201=1%20--
        /// </summary>
        /// <param name="studentFirstMidName"></param>
        /// <returns></returns>
        public IActionResult Example100_FindByStudentFirstMidName(string studentFirstMidName)
        {
            return Content(FindStudent_V1(null,null, studentFirstMidName));
        }

        /// <summary>
        /// SQL injection
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="studentLastName"></param>
        /// <param name="studentFirstMidName"></param>
        /// <returns></returns>
        private string FindStudent_V1(string studentId, string studentLastName, string studentFirstMidName)
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

        public IActionResult FindStudent_V1_simplified(string studentLastName)
        {
            SqliteConnection conn = new SqliteConnection(configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            string commandText = "select ID,LastName,FirstMidName,EnrollmentDate from Student where LastName='" + studentLastName + "'";

            SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText = commandText;
            SqliteDataReader reader = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();

            while (reader.Read())
            {
                sb.Append("ID: ").Append(reader[0]).AppendLine();
                sb.Append("LastName: ").Append(reader[1]).AppendLine();
                sb.Append("FirstMidName: ").Append(reader[2]).AppendLine();
                sb.Append("EnrollmentDate: ").Append(reader[3]).AppendLine();
            }

            return Content(sb.ToString());
        }

        /// <summary>
        /// mitigate SQL injection: use stored procedure
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="studentLastName"></param>
        /// <param name="studentFirstMidName"></param>
        /// <returns></returns>
        private string FindStudent_V2(string studentId, string studentLastName, string studentFirstMidName)
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

        private string FindStudent_V2_Simplified(string studentId, string studentLastName, string studentFirstMidName)
        {
            using (SqliteConnection conn = new SqliteConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();

                SqliteCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select ID,LastName,FirstMidName,EnrollmentDate from Student where LastName = @LastName";
                cmd.Parameters.Add("LastName", SqliteType.Text);
                cmd.Parameters["LastName"].Value = studentLastName;

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
    }
}
