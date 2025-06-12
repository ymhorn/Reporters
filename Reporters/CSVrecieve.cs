using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Reporters
{
    //A way to recieve reports through a CSV file
    internal static class CSVrecieve
    {
        //Recieves data from the CSV file and creates a list of report objects
        public static List<Report> RecieveCSV(string fullPath)
        {
            List<Report> fullList = new List<Report>();

            //Reads the CSV file
            using (var reader = new StreamReader(fullPath))
            {
                while (!reader.EndOfStream)
                {
                    //Split each line and create report object from the data
                    string line = reader.ReadLine();
                    string[] items = line.Split(',');
                    Report report = new Report();
                    report.ReporterName = items[0];
                    report.ReportedName = items[1];
                    report.ReportText = items[2];
                    report.DateTime = DateTime.Parse(items[3]);

                    //Adds each report to the list
                    fullList.Add(report);
                }
                
            }
            return fullList;
        }
    }
}
