using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Reporters
{
    //A way to find the real name ofan individual
    internal static class FindName
    {
        //URL to DataBase
        static private string connectionString = "server = localhost; user = root; database = reportdb; port = 3306;";
        
        //Recieves code name or id number and outputs his real name
        public static void Name(string codeName)
        {
            //Checks permissions
            if (ValidateUser.Password())
            {
                //Connect to URL
                MySqlConnection connect = new MySqlConnection(connectionString);

                connect.Open();
                try
                {
                    //SQL query to recieve the real name of the individual
                    string query = "SELECT name FROM personalinfo WHERE codename = @codename;";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@codename", codeName);
                    string name = Convert.ToString(cmd.ExecuteScalar());

                    //Output real name
                    Console.WriteLine(name);
                }
                //Output when code name or id is not found
                catch (Exception)
                {
                    Console.WriteLine("No such code name found.");
                }
            }
            //Output when user doesn't have permission
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
