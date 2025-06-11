using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Reporters
{
    internal static class PotentialCandidates
    {
        static private string connectionString = "server = localhost; user = root; database = reportdb; port = 3306;";

        public static void ListCandidates()
        {
            MySqlConnection connect = new MySqlConnection(connectionString);

            connect.Open();

            List<string> candidates = new List<string>();

            string query = "SELECT name FROM personalinfo JOIN personalreports ON " +
                "personalinfo.id = personalreports.personid WHERE personalreports.longreports > 10;";
            MySqlCommand command = new MySqlCommand(query, connect);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                candidates.Add(reader.GetString("name"));
            }

            reader.Close();
            connect.Close();

            foreach (string candidate in candidates)
            {
                Console.WriteLine(candidate);
            }

        }
    }
}
