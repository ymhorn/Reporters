using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reporters
{
    internal static class InputReport
    {
        public static Report Input()
        {
            Report report = new Report();
            Console.WriteLine("Enter your name/code name:");
            report.ReporterName = Console.ReadLine();
            Console.WriteLine("Who are you reporting about:");
            report.ReportedName = Console.ReadLine();
            Console.WriteLine("What is the report:");
            report.ReportText = Console.ReadLine();
            report.DateTime = DateTime.Now;

            return report;
        }
    }
}
