using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Reporters
{
    internal static class SearchByName
    {
        static private string connectionString = "server = localhost; user = root; database = reportdb; port = 3306;";

        public static void NameInfo(string name)
        {
            MySqlConnection connect = new MySqlConnection(connectionString);

            connect.Open();

            try
            {
                string query = "SELECT longreports, shorttimealerts + manyalerts as alerts FROM personalreports " +
                    "JOIN personalinfo ON personalreports.personid = personalinfo.id WHERE personalinfo.name = @name " +
                    "OR personalinfo.codename = @name; ";
                MySqlCommand cmd = new MySqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@name", name);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                int reports = reader.GetInt32("longreports");
                int alerts = reader.GetInt32("alerts");
                reader.Close();
                connect.Close();

                if (reports > 0 && alerts > 0)
                {
                    Console.WriteLine("This guy is some sort of double agent!!");
                }
                else if (reports > 0)
                {
                    Console.WriteLine("This guy is a good potential candidate to hire.");
                }
                else if (alerts > 0)
                {
                    Console.WriteLine("This guy is dangerous, be careful with him.");
                }
                else
                {
                    Console.WriteLine("Not enough information about this guy.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("This guy doesn't exist in the database.");
            }






        }


    }
}
