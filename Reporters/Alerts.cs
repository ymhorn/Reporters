using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Reporters
{
    //A way to find out all current alerts
    internal static class Alerts
    {
        //URL to DataBase
        static private string connectionString = "server = localhost; user = root; database = reportdb; port = 3306;";

        //Outputs information about the alerts
        public static void AllAlerts()
        {
            //Checks permissions
            if (ValidateUser.Password())
            {
                Dictionary<string, int> alerts = new Dictionary<string, int>();

                //Connect to URL
                MySqlConnection connect = new MySqlConnection(connectionString);

                connect.Open();

                //SQL query to recieve all the alerts about the people
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

                //Output name and amount of alerts
                foreach (var alert in alerts)
                {
                    Console.WriteLine($"{alert.Value} alerts about {alert.Key}");
                }
            }
            //Output when user doesn't have permission
            else
            {
                Console.WriteLine("You do not have permission to see this data");
            }
        }

    }
}
