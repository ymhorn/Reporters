using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporters
{
    //A menu to display all options to user and calls the relevant function
    internal static class Menu
    {
        //Runs the menu
        public static void RunMenu()
        {
            bool exit = false;
            while (!exit)
            {
                //Display all options to the user
                Console.WriteLine("\nWhat would you like to do today:\n" +
                    "1. Add a report to the system.\n" +
                    "2. Add a CSV file with reports to the system.\n" +
                    "3. Find potential candidates to hire.\n" +
                    "4. Find out all alerts.\n" +
                    "5. Find out what a certain person is.\n" +
                    "6. Find out the real name of a person.\n" +
                    "7. Exit\n");

                //Recieve option
                string option = Console.ReadLine();

                //Runs the program based on the user input
                switch (option)
                {
                    case "1":
                        ReportToDB.AddReport(InputReport.Input());
                        break;
                    case "2":
                        Console.WriteLine("What is the path to your file.");
                        string path = Console.ReadLine();
                        ReportToDB.AddReport(CSVrecieve.RecieveCSV(path));
                        break;
                    case "3":
                        PotentialCandidates.ListCandidates();
                        break;
                    case "4":
                        Alerts.AllAlerts();
                        break;
                    case "5":
                        Console.WriteLine("What name are you searching.");
                        string name = Console.ReadLine();
                        SearchByName.NameInfo(name);
                        break;
                    case "6":
                        Console.WriteLine("Do you know his code name or id?");
                        string answer = Console.ReadLine();
                        if (answer == "code name")
                        {
                            Console.WriteLine("What is the code name?");
                            string codeName = Console.ReadLine();
                            FindName.Name(codeName);
                        }
                        else if (answer == "id")
                        {
                            Console.WriteLine("What is his id?");
                            int id = int.Parse(Console.ReadLine());
                            FindName.Name(id);
                        }
                        else
                        {
                            Console.WriteLine("That was not an option!");
                        }
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Sorry, no such option available");
                        break;
                
                }

            }
        }
    }
}
