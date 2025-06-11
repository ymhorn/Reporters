using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Reporters
{
    internal static class ReportToDB
    {
        static private string connectionString = "server = localhost; user = root; database = reportdb; port = 3306;";

        public static void AddReport(Report report)
        {
            MySqlConnection connect = new MySqlConnection(connectionString);

            connect.Open();

            string query1 = "SELECT EXISTS (SELECT 1 FROM personalinfo WHERE name = @reporterName OR codename = @reporterName);";
            MySqlCommand cmd1 = new MySqlCommand(query1, connect);
            cmd1.Parameters.AddWithValue("@reporterName", report.ReporterName);
            object result1 = cmd1.ExecuteScalar();
            bool exists = Convert.ToBoolean(result1);

            if (!exists)
            {
                string query2 = "INSERT INTO personalinfo(name) VALUES (@reporterName)";
                MySqlCommand cmd2 = new MySqlCommand(query2, connect);
                cmd2.Parameters.AddWithValue("@reporterName", report.ReporterName);
                cmd2.ExecuteNonQuery();
            }

            string query3 = "SELECT EXISTS (SELECT 1 FROM personalinfo WHERE (name = @reporterName OR codename = @reporterName) AND codename IS NOT NULL);";
            MySqlCommand cmd3 = new MySqlCommand(query3, connect);
            cmd3.Parameters.AddWithValue("@reporterName", report.ReporterName);
            object result3 = cmd3.ExecuteScalar();
            bool hasCodename = Convert.ToBoolean(result3);
            if (!hasCodename)
            {
                Console.WriteLine("What would you like your codename to be:");
                string codeName = Console.ReadLine();
                string query4 = "UPDATE personalinfo SET codename = @codeName WHERE name = @reporterName";               
                MySqlCommand cmd4 = new MySqlCommand(query4, connect);
                cmd4.Parameters.AddWithValue("@reporterName", report.ReporterName);
                cmd4.Parameters.AddWithValue("@codeName", codeName);
                cmd4.ExecuteNonQuery();
            }
            string query5 = "SELECT EXISTS (SELECT 1 FROM personalinfo WHERE name = @reportedName OR codename = @reportedName);";
            MySqlCommand cmd5 = new MySqlCommand(query5, connect);
            cmd5.Parameters.AddWithValue("@reportedName", report.ReportedName);
            object result5 = cmd5.ExecuteScalar();
            bool exists5 = Convert.ToBoolean(result5);
            if (!exists5)
            {
                string query6 = "INSERT INTO personalinfo(Name) VALUES (@reportedName)";
                MySqlCommand cmd6 = new MySqlCommand(query6, connect);
                cmd6.Parameters.AddWithValue("@reportedName", report.ReportedName);
                cmd6.ExecuteNonQuery();
            }
            string query7 = "SELECT id from personalinfo WHERE personalinfo.name = @reporterName OR personalinfo.codename = @reporterName";
            MySqlCommand cmd7 = new MySqlCommand(query7, connect);
            cmd7.Parameters.AddWithValue("@reporterName", report.ReporterName);
            int reporterID = Convert.ToInt32(cmd7.ExecuteScalar());


            string query8 = "SELECT id from personalinfo WHERE personalinfo.name = @reportedName OR personalinfo.codename = @reportedName";
            MySqlCommand cmd8 = new MySqlCommand(query8, connect);
            cmd8.Parameters.AddWithValue("@reportedName", report.ReportedName);
            int reportedID = Convert.ToInt32(cmd8.ExecuteScalar());


            int reportLength = report.ReportText.Length;
            string query9 = "INSERT INTO reports (reporterid,reportedid," +
                "report,lengthreport,reporttime) VALUES (@reporterName," +
                "@reportedName,@report,@length,@time)";
            MySqlCommand cmd9 = new MySqlCommand(query9, connect);
            cmd9.Parameters.AddWithValue("@reporterName", reporterID);
            cmd9.Parameters.AddWithValue("@reportedName", reportedID);
            cmd9.Parameters.AddWithValue("@report", report.ReportText);
            cmd9.Parameters.AddWithValue("@time", report.DateTime);
            cmd9.Parameters.AddWithValue("@length", reportLength);
            cmd9.ExecuteNonQuery();

            

        }

       

        public static void AddReport(List<Report> reports)
        {
            MySqlConnection connect = new MySqlConnection(connectionString);
            connect.Open();

            foreach (Report report  in reports)
            {
                string query1 = "SELECT EXISTS (SELECT 1 FROM personalinfo WHERE name = @reporterName OR codename = @reporterName);";
                MySqlCommand cmd1 = new MySqlCommand(query1, connect);
                cmd1.Parameters.AddWithValue("@reporterName", report.ReporterName);
                object result1 = cmd1.ExecuteScalar();
                bool exists = Convert.ToBoolean(result1);

                if (!exists)
                {
                    string query2 = "INSERT INTO personalinfo(name) VALUES (@reporterName)";
                    MySqlCommand cmd2 = new MySqlCommand(query2, connect);
                    cmd2.Parameters.AddWithValue("@reporterName", report.ReporterName);
                    cmd2.ExecuteNonQuery();
                }
                string query5 = "SELECT EXISTS (SELECT 1 FROM personalinfo WHERE name = @reportedName OR codename = @reportedName);";
                MySqlCommand cmd5 = new MySqlCommand(query5, connect);
                cmd5.Parameters.AddWithValue("@reportedName", report.ReportedName);
                object result5 = cmd5.ExecuteScalar();
                bool exists5 = Convert.ToBoolean(result5);
                if (!exists5)
                {
                    string query6 = "INSERT INTO personalinfo(Name) VALUES (@reportedName)";
                    MySqlCommand cmd6 = new MySqlCommand(query6, connect);
                    cmd6.Parameters.AddWithValue("@reportedName", report.ReportedName);
                    cmd6.ExecuteNonQuery();
                }
                string query7 = "SELECT id from personalinfo WHERE personalinfo.name = @reporterName OR personalinfo.codename = @reporterName";
                MySqlCommand cmd7 = new MySqlCommand(query7, connect);
                cmd7.Parameters.AddWithValue("@reporterName", report.ReporterName);
                int reporterID = Convert.ToInt32(cmd7.ExecuteScalar());


                string query8 = "SELECT id from personalinfo WHERE personalinfo.name = @reportedName OR personalinfo.codename = @reportedName";
                MySqlCommand cmd8 = new MySqlCommand(query8, connect);
                cmd8.Parameters.AddWithValue("@reportedName", report.ReportedName);
                int reportedID = Convert.ToInt32(cmd8.ExecuteScalar());


                int reportLength = report.ReportText.Length;
                string query9 = "INSERT INTO reports (reporterid,reportedid," +
                    "report,lengthreport,reporttime) VALUES (@reporterName," +
                    "@reportedName,@report,@length,@time)";
                MySqlCommand cmd9 = new MySqlCommand(query9, connect);
                cmd9.Parameters.AddWithValue("@reporterName", reporterID);
                cmd9.Parameters.AddWithValue("@reportedName", reportedID);
                cmd9.Parameters.AddWithValue("@report", report.ReportText);
                cmd9.Parameters.AddWithValue("@time", report.DateTime);
                cmd9.Parameters.AddWithValue("@length", reportLength);
                cmd9.ExecuteNonQuery();

            }

        

        }
    }
}
