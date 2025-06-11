using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Reporters
{
    internal static class Alerts
    {
        static private string connectionString = "server = localhost; user = root; database = reportdb; port = 3306;";

        public static void AllAlerts()
        {
            if (ValidateUser.Password())
            {
                MySqlConnection connect = new MySqlConnection(connectionString);

                connect.Open();

                Dictionary<string, int> alerts = new Dictionary<string, int>();

                string query = "SELECT name, shorttimealerts + manyalerts AS alerts FROM personalinfo " +
                    "JOIN personalreports ON personalinfo.id = personalreports.personid WHERE shorttimealerts + manyalerts > 0;";
                MySqlCommand cmd = new MySqlCommand(query, connect);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    alerts.Add(reader.GetString("name"), reader.GetInt32("alerts"));
                }
                reader.Close();
                connect.Close();

                foreach (var alert in alerts)
                {
                    Console.WriteLine($"{alert.Value} alerts about {alert.Key}");
                }
            }
            else
            {
                Console.WriteLine("You do not have permission to see this data");
            }
        }

    }
}
