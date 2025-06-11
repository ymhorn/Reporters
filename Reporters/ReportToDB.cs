using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            bool exists = Convert.ToBoolean(cmd1.ExecuteScalar());

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
            bool hasCodename = Convert.ToBoolean(cmd3.ExecuteScalar());
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
            bool exists5 = Convert.ToBoolean(cmd5.ExecuteScalar());
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

            string query10 = "SELECT COUNT(*) FROM reports WHERE reportedid = @reportedid;";
            MySqlCommand cmd10 = new MySqlCommand(query10, connect);
            cmd10.Parameters.AddWithValue("@reportedid", reportedID);
            int timesReported = Convert.ToInt32(cmd10.ExecuteScalar());

            string query11 = "SELECT COUNT(*) FROM reports WHERE reporterid = @reporterid AND lengthreport > 100;";
            MySqlCommand cmd11 = new MySqlCommand(query11, connect);
            cmd11.Parameters.AddWithValue("@reporterid",reporterID);
            int longReports = Convert.ToInt32(cmd11.ExecuteScalar());

            string query12 = "SELECT COUNT(*) FROM reports WHERE reporterid = @reporterid AND lengthreport < 100;";
            MySqlCommand cmd12 = new MySqlCommand(query12, connect);
            cmd12.Parameters.AddWithValue("@reporterid", reporterID);
            int shortReports = Convert.ToInt32(cmd12.ExecuteScalar());

            string query13 = "SELECT COUNT(*) FROM reports WHERE reportedid = @reportedid AND reporttime BETWEEN DATE_SUB(@datetime, INTERVAL 15 MINUTE) AND @datetime;";
            MySqlCommand cmd13 = new MySqlCommand(query13, connect);
            cmd13.Parameters.AddWithValue("@reportedid", reportedID);
            cmd13.Parameters.AddWithValue("@datetime", report.DateTime);
            int alertsShortTime = Convert.ToInt32(cmd13.ExecuteScalar());
            int shortAlert;
            if (alertsShortTime > 2)
            {
                shortAlert = 1;
            }
            else
            {
                shortAlert = 0;
            }

            string query14 = "SELECT EXISTS (SELECT 1 FROM personalreports WHERE personid = @personid);";
            MySqlCommand cmd14 = new MySqlCommand(query14, connect);
            cmd14.Parameters.AddWithValue("@personid", reporterID);
            bool existReporter = Convert.ToBoolean(cmd14.ExecuteScalar());

            if (existReporter)
            {
                string query15 = "UPDATE personalreports SET longreports = @longreports," +
                    "shortreports = @shortreports WHERE personid = @personid";
                MySqlCommand cmd15 = new MySqlCommand(query15, connect);
                cmd15.Parameters.AddWithValue("@longreports", longReports);
                cmd15.Parameters.AddWithValue("@shortreports", shortReports);
                cmd15.Parameters.AddWithValue("@personid", reporterID);
                cmd15.ExecuteNonQuery();
            }
            else
            {
                string query16 = "INSERT INTO personalreports(personid,longreports,shortreports)" +
                    "VALUES(@personid,@longreports,@shortreports)";
                MySqlCommand cmd16 = new MySqlCommand(query16, connect);
                cmd16.Parameters.AddWithValue("@longreports", longReports);
                cmd16.Parameters.AddWithValue("@shortreports", shortReports);
                cmd16.Parameters.AddWithValue("@personid", reporterID);
                cmd16.ExecuteNonQuery();
            }

            string query17 = "SELECT EXISTS (SELECT 1 FROM personalreports WHERE personid = @personid);";
            MySqlCommand cmd17 = new MySqlCommand(query17, connect);
            cmd17.Parameters.AddWithValue("@personid", reportedID);
            bool existReported = Convert.ToBoolean(cmd17.ExecuteScalar());

            if (existReported)
            {
                string query18 = "UPDATE personalreports SET timesreported = @timesreported," +
                    "shorttimealerts = shorttimealerts + @shortalert, manyalerts = @timesreported / 20 " +
                    "WHERE personid = @personid;";
                MySqlCommand cmd18 = new MySqlCommand(query18, connect);
                cmd18.Parameters.AddWithValue("@timesreported",timesReported);
                cmd18.Parameters.AddWithValue("@shortalert", shortAlert);
                cmd18.Parameters.AddWithValue("@personid", reportedID);
                cmd18.ExecuteNonQuery();

            }
            else
            {
                string query19 = "INSERT INTO personalreports(personid,timesreported,shorttimealerts," +
                    "manyalerts) VALUES(@personid,@timesreported,0,0);";
                MySqlCommand cmd19 = new MySqlCommand(query19, connect);
                cmd19.Parameters.AddWithValue("@personid", reportedID);
                cmd19.Parameters.AddWithValue("@timesreported", timesReported);
                cmd19.ExecuteNonQuery();

            }
            connect.Close();

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
                bool exists = Convert.ToBoolean(cmd1.ExecuteScalar());

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
                bool exists5 = Convert.ToBoolean(cmd5.ExecuteScalar());
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

                string query10 = "SELECT COUNT(*) FROM reports WHERE reportedid = @reportedid;";
                MySqlCommand cmd10 = new MySqlCommand(query10, connect);
                cmd10.Parameters.AddWithValue("@reportedid", reportedID);
                int timesReported = Convert.ToInt32(cmd10.ExecuteScalar());

                string query11 = "SELECT COUNT(*) FROM reports WHERE reporterid = @reporterid AND lengthreport > 100;";
                MySqlCommand cmd11 = new MySqlCommand(query11, connect);
                cmd11.Parameters.AddWithValue("@reporterid", reporterID);
                int longReports = Convert.ToInt32(cmd11.ExecuteScalar());

                string query12 = "SELECT COUNT(*) FROM reports WHERE reporterid = @reporterid AND lengthreport < 100;";
                MySqlCommand cmd12 = new MySqlCommand(query12, connect);
                cmd12.Parameters.AddWithValue("@reporterid", reporterID);
                int shortReports = Convert.ToInt32(cmd12.ExecuteScalar());

                string query13 = "SELECT COUNT(*) FROM reports WHERE reportedid = @reportedid AND reporttime BETWEEN DATE_SUB(@datetime, INTERVAL 15 MINUTE) AND @datetime;";
                MySqlCommand cmd13 = new MySqlCommand(query13, connect);
                cmd13.Parameters.AddWithValue("@reportedid", reportedID);
                cmd13.Parameters.AddWithValue("@datetime", report.DateTime);
                int alertsShortTime = Convert.ToInt32(cmd13.ExecuteScalar());
                int shortAlert;
                if (alertsShortTime > 2)
                {
                    shortAlert = 1;
                }
                else
                {
                    shortAlert = 0;
                }

                string query14 = "SELECT EXISTS (SELECT 1 FROM personalreports WHERE personid = @personid);";
                MySqlCommand cmd14 = new MySqlCommand(query14, connect);
                cmd14.Parameters.AddWithValue("@personid", reporterID);
                bool existReporter = Convert.ToBoolean(cmd14.ExecuteScalar());

                if (existReporter)
                {
                    string query15 = "UPDATE personalreports SET longreports = @longreports," +
                        "shortreports = @shortreports WHERE personid = @personid";
                    MySqlCommand cmd15 = new MySqlCommand(query15, connect);
                    cmd15.Parameters.AddWithValue("@longreports", longReports);
                    cmd15.Parameters.AddWithValue("@shortreports", shortReports);
                    cmd15.Parameters.AddWithValue("@personid", reporterID);
                    cmd15.ExecuteNonQuery();
                }
                else
                {
                    string query16 = "INSERT INTO personalreports(personid,longreports,shortreports)" +
                        "VALUES(@personid,@longreports,@shortreports)";
                    MySqlCommand cmd16 = new MySqlCommand(query16, connect);
                    cmd16.Parameters.AddWithValue("@longreports", longReports);
                    cmd16.Parameters.AddWithValue("@shortreports", shortReports);
                    cmd16.Parameters.AddWithValue("@personid", reporterID);
                    cmd16.ExecuteNonQuery();
                }

                string query17 = "SELECT EXISTS (SELECT 1 FROM personalreports WHERE personid = @personid);";
                MySqlCommand cmd17 = new MySqlCommand(query17, connect);
                cmd17.Parameters.AddWithValue("@personid", reportedID);
                bool existReported = Convert.ToBoolean(cmd17.ExecuteScalar());

                if (existReported)
                {
                    string query18 = "UPDATE personalreports SET timesreported = @timesreported," +
                        "shorttimealerts = shorttimealerts + @shortalert, manyalerts = @timesreported / 20 " +
                        "WHERE personid = @personid;";
                    MySqlCommand cmd18 = new MySqlCommand(query18, connect);
                    cmd18.Parameters.AddWithValue("@timesreported", timesReported);
                    cmd18.Parameters.AddWithValue("@shortalert", shortAlert);
                    cmd18.Parameters.AddWithValue("@personid", reportedID);
                    cmd18.ExecuteNonQuery();

                }
                else
                {
                    string query19 = "INSERT INTO personalreports(personid,timesreported,shorttimealerts," +
                        "manyalerts) VALUES(@personid,@timesreported,0,0);";
                    MySqlCommand cmd19 = new MySqlCommand(query19, connect);
                    cmd19.Parameters.AddWithValue("@personid", reportedID);
                    cmd19.Parameters.AddWithValue("@timesreported", timesReported);
                    cmd19.ExecuteNonQuery();

                }

            }
            connect.Close();

        }
    }
}
