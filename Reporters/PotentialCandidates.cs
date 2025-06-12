using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Reporters
{
    //A way to recieve a list of candidates that would be good reporters
    internal static class PotentialCandidates
    {
        //URL to DataBase
        static private string connectionString = "server = localhost; user = root; database = reportdb; port = 3306;";

        //Output all potential candidates
        public static void ListCandidates()
        {
            //Checks permissions
            if (ValidateUser.Password())
            {
                List<string> candidates = new List<string>();

                //Connect to URL
                MySqlConnection connect = new MySqlConnection(connectionString);

                connect.Open();

                //SQL query to return all candidates that fit the criteria
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

                //Outputs all the names
                foreach (string candidate in candidates)
                {
                    Console.WriteLine(candidate);
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
