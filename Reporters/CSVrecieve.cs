using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Reporters
{
    internal static class CSVrecieve
    {
        public static List<Report> RecieveCSV(string fullPath)
        {
            List<Report> fullList = new List<Report>();

            using (var reader = new StreamReader(fullPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] items = line.Split(',');
                    Report report = new Report();
                    report.ReporterName = items[0];
                    report.ReportedName = items[1];
                    report.ReportText = items[2];
                    report.DateTime = DateTime.Parse(items[3]);
                    fullList.Add(report);
                }
                
            }
            return fullList;
        }
    }
}
