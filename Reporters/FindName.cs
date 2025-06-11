using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Reporters
{
    internal static class FindName
    {
        static private string connectionString = "server = localhost; user = root; database = reportdb; port = 3306;";
        
        public static void Name(string codeName)
        {
            if (ValidateUser.Password())
            {
                MySqlConnection connect = new MySqlConnection(connectionString);

                connect.Open();
                try
                {
                    string query = "SELECT name FROM personalinfo WHERE codename = @codename;";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@codename", codeName);
                    string name = Convert.ToString(cmd.ExecuteScalar());
                    Console.WriteLine(name);
                }
                catch (Exception)
                {
                    Console.WriteLine("No such code name found.");
                }
            }
            else
            {
                Console.WriteLine("You do not have permission to see this data");
            }
        }

        public static void Name(int id)
        {
            if (ValidateUser.Password())
            {
                MySqlConnection connect = new MySqlConnection(connectionString);

                connect.Open();
                try
                {
                    string query = "SELECT name FROM personalinfo WHERE id = @id;";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@id", id);
                    string name = Convert.ToString(cmd.ExecuteScalar());
                    Console.WriteLine(name);
                }
                catch
                {
                    Console.WriteLine("No such id found");
                }
            }
            else
            { 
                Console.WriteLine("You do not have permission to see this data"); 
            }

        }
    

    }
}
